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
        I2cDevice _tsl2561;
        SpiDevice _spiDevice;
        GpioPin _shdn;
        GpioPin _heater;
        int leds = 8;
        byte[] endFrame;
        int _color = 0;
        bool _up = false;

        Clock _clock;
        BME280 _bme280;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _taskInstance = taskInstance;
            _deferral = _taskInstance.GetDeferral();
            _taskInstance.Canceled += TaskInstance_Canceled;
            var gpio = GpioController.GetDefault();

            // turn off red en green led
            var red = gpio.OpenPin(35);
            red.Write(GpioPinValue.Low);
            red.SetDriveMode(GpioPinDriveMode.Output);
            var green = gpio.OpenPin(47);
            green.Write(GpioPinValue.Low);
            green.SetDriveMode(GpioPinDriveMode.Output);

            // power cycle +3v3 to reset all I2C devices
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

            _bme280 = new BME280();
            await _bme280.Init();

            _timer2 = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick2, TimeSpan.FromMilliseconds(4));
            //_timer3 = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick3, TimeSpan.FromMilliseconds(1000));
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
                endFrame[i] = 0x0;
            }

            // start
            _spiDevice.Write(new byte[4]);

            // all off
            _spiDevice.Write(new byte[] { 0xE0, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xE0, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xE0, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xE0, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xE0, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xE0, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xE0, 0x0, 0x0, 0x0 });
            _spiDevice.Write(new byte[] { 0xE0, 0x0, 0x0, 0x0 });

            // end
            _spiDevice.Write(endFrame);
        }

        private byte _brightness = 0xE0 + 10;

        private void Timer_Tick2(ThreadPoolTimer timer)
        {
            if (_spiDevice != null)
            {
                try
                {
                    var color = HsvToRgb(_color, 255, 255);
                    _spiDevice.Write(new byte[4]);
                    for(var i=0; i < 8; i++)
                    {
                        
                        _spiDevice.Write(new byte[] { _brightness, color.B, color.G, color.R });
                    }
                    _spiDevice.Write(endFrame);
                    _color++;
                    if (_color >= 360)
                        _color = 0;
                }
                catch (Exception)
                {

                }
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
                _bme280.Sample();
                Debug.WriteLine($"temperature: {_bme280.Temperature} DegC");
                Debug.WriteLine($"pressure: {_bme280.Pressure} Pa");
                Debug.WriteLine($"humidity: {_bme280.Humidity} %rH");
            }

            if (_tsl2561 != null)
            {
                var buffer = new byte[2];
                _tsl2561.WriteRead(new byte[] { 0xAC }, buffer);
                Debug.WriteLine("light: " + 256 * buffer[1] + buffer[0]);
            }
        }

        Color HsvToRgb(int h, byte s, byte v)
        {
            var f = (h % 60) * 255 / 60;
            var p = (255 - s) * v / 255;
            var q = (255 - f * s / 255) * v / 255;
            var t = (255 - (255 - f) * s / 255) * v / 255;
            byte r = 0, g = 0, b = 0;
            switch ((h / 60) % 6)
            {
                case 0: r = (byte)v; g = (byte)t; b = (byte)p; break;
                case 1: r = (byte)q; g = (byte)v; b = (byte)p; break;
                case 2: r = (byte)p; g = (byte)v; b = (byte)t; break;
                case 3: r = (byte)p; g = (byte)q; b = (byte)v; break;
                case 4: r = (byte)t; g = (byte)p; b = (byte)v; break;
                case 5: r = (byte)v; g = (byte)p; b = (byte)q; break;
            }

            return Color.FromArgb(255, r, g, b);
        }
    }
}
