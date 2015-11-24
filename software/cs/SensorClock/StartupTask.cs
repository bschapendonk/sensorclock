using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;
using Windows.Devices.Spi;
using Windows.System.Threading;
using Windows.UI;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SensorClock
{
    public sealed class StartupTask : IBackgroundTask
    {
        IBackgroundTaskInstance _taskInstance;
        BackgroundTaskDeferral _deferral;

        ThreadPoolTimer _timer2;
        ThreadPoolTimer _timer3;

        I2cDevice _mcp3425;
        I2cDevice _bme280;
        I2cDevice _tsl2561;
        SpiDevice _spiDevice;
        GpioPin _shdn;
        GpioPin _heater;
        int leds = 8;
        byte[] endFrame;
        int _color = 0;
        bool _up = false;

        Clock _clock;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _taskInstance = taskInstance;
            _deferral = _taskInstance.GetDeferral();
            _taskInstance.Canceled += TaskInstance_Canceled;

            // power cycle +3v3 to reset all I2C devices
            var gpio = GpioController.GetDefault();
            _shdn = gpio.OpenPin(5);
            _shdn.Write(GpioPinValue.High);
            _shdn.SetDriveMode(GpioPinDriveMode.Output);
            _shdn.Write(GpioPinValue.Low);
            // wait to discharge all capacitors
            Task.Delay(3000).GetAwaiter().GetResult();
            _shdn.Write(GpioPinValue.High);

            _heater = gpio.OpenPin(16);
            _heater.Write(GpioPinValue.Low);
            _heater.SetDriveMode(GpioPinDriveMode.Output);

            _clock = new Clock();
            _clock.Start();

            await Init();
            await _clock.Init();

            _timer2 = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick2, TimeSpan.FromMilliseconds(100));
            _timer3 = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick3, TimeSpan.FromMilliseconds(1000));
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _clock.Dispose();
            _timer2.Cancel();
            _timer3.Cancel();
            _deferral.Complete();
        }

        private async Task Init()
        {
            var deviceSelector = I2cDevice.GetDeviceSelector();
            var controllers = await DeviceInformation.FindAllAsync(deviceSelector);

            _mcp3425 = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(0x68) { BusSpeed = I2cBusSpeed.FastMode });
            _mcp3425.Write(new byte[] { 0x98 }); // 16bit

            _bme280 = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(0x77) { BusSpeed = I2cBusSpeed.FastMode });
            _bme280.Write(new byte[] { 0xF4, 0x4B }); // power on :)

            _tsl2561 = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(0x39) { BusSpeed = I2cBusSpeed.FastMode });
            _tsl2561.Write(new byte[] { 0x80, 0x03 }); // power on :)

            // spi stuff
            var settings = new SpiConnectionSettings(0);
            settings.ClockFrequency = 10000000;
            settings.Mode = SpiMode.Mode0;

            string spiAqs = SpiDevice.GetDeviceSelector("SPI0");
            var deviceInformation = await DeviceInformation.FindAllAsync(spiAqs);
            _spiDevice = await SpiDevice.FromIdAsync(deviceInformation[0].Id, settings);
            endFrame = new byte[(int)Math.Ceiling(leds / 16d)];
            for (var i = 0; i < endFrame.Length; i++)
            {
                endFrame[i] = 0xFF;
            }

            // start
            _spiDevice.Write(new byte[4]);

            // all off
            _spiDevice.Write(new byte[] { 0xFF, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xFF, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xFF, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xFF, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xFF, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xFF, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xFF, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xFF, 0x0, 0x0, 0x0 });

            // end
            _spiDevice.Write(endFrame);
        }

        private void Timer_Tick2(ThreadPoolTimer timer)
        {
            if (_spiDevice != null)
            {
                var color = SpectrumColor(_color, 50);
                _spiDevice.Write(new byte[4]);
                _spiDevice.Write(new byte[] { 0xFF, color.B, color.G, color.R });
                _spiDevice.Write(new byte[] { 0xFF, color.B, color.G, color.R });
                _spiDevice.Write(new byte[] { 0xFF, color.B, color.G, color.R });
                _spiDevice.Write(new byte[] { 0xFF, color.B, color.G, color.R });
                _spiDevice.Write(new byte[] { 0xFF, color.B, color.G, color.R });
                _spiDevice.Write(new byte[] { 0xFF, color.B, color.G, color.R });
                _spiDevice.Write(new byte[] { 0xFF, color.B, color.G, color.R });
                _spiDevice.Write(new byte[] { 0xFF, color.B, color.G, color.R });
                _spiDevice.Write(endFrame);

                if (_up)
                    _color++;
                else
                    _color--;

                if (_color >= 100)
                    _up = false;
                if (_color <= 0)
                    _up = true;
            }
        }

        private void Timer_Tick3(ThreadPoolTimer timer)
        {
            if (_mcp3425 != null)
            {
                var buffer = new byte[3];
                _mcp3425.Read(buffer);
                var voltage = (256 * buffer[0] + buffer[1]) * 0.0000625;
                Debug.WriteLine("adc: " + voltage);
            }

            if (_bme280 != null)
            {
                var buffer = new byte[1];
                _bme280.WriteRead(new byte[] { 0xD0 }, buffer);
                if (buffer[0] == 0x60)
                {
                    // chip id ok
                }
                var h = new byte[1];
                var l = new byte[1];
                _bme280.WriteRead(new byte[] { 0xFA }, h);
                _bme280.WriteRead(new byte[] { 0xFB }, l);
                Debug.WriteLine("temp: " + 256 * h[0] + l[0]);
            }

            if (_tsl2561 != null)
            {
                var buffer = new byte[2];
                _tsl2561.WriteRead(new byte[] { 0xAC }, buffer);
                Debug.WriteLine("light: " + 256 * buffer[1] + buffer[0]);
            }
        }

        private static Color SpectrumColor(int w, int brightness)
        {
            float r = 0.0f;
            float g = 0.0f;
            float b = 0.0f;

            w = w % 100;

            if (w < 17)
            {
                r = -(w - 17.0f) / 17.0f;
                b = 1.0f;
            }
            else if (w < 33)
            {
                g = (w - 17.0f) / (33.0f - 17.0f);
                b = 1.0f;
            }
            else if (w < 50)
            {
                g = 1.0f;
                b = -(w - 50.0f) / (50.0f - 33.0f);
            }
            else if (w < 67)
            {
                r = (w - 50.0f) / (67.0f - 50.0f);
                g = 1.0f;
            }
            else if (w < 83)
            {
                r = 1.0f;
                g = -(w - 83.0f) / (83.0f - 67.0f);
            }
            else
            {
                r = 1.0f;
                b = (w - 83.0f) / (100.0f - 83.0f);
            }

            return Color.FromArgb(255, (byte)(r * brightness), (byte)(g * brightness), (byte)(b * brightness));
        }
    }
}
