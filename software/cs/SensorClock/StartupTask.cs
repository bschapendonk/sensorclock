using System;
using System.Diagnostics;
using System.Net.Http;
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
        private BME280 _bme280;
        private byte _brightness = 0xE0 | 31;
        private Clock _clock;
        private int _hue = 0;

        private int _hue1 = 0;
        private int _hue2 = 45;
        private int _hue3 = 90;
        private int _hue4 = 135;
        private int _hue5 = 180;
        private int _hue6 = 225;
        private int _hue7 = 270;
        private int _hue8 = 315;
        private BackgroundTaskDeferral _deferral;
        private GpioPin _heater;
        private I2cDevice _mcp3425;
        private GpioPin _shdn;
        private SpiDevice _spiDevice;
        private IBackgroundTaskInstance _taskInstance;
        private ThreadPoolTimer _timer2;
        private ThreadPoolTimer _timer3;
        private I2cDevice _tsl2561;
        private bool _up = false;
        private byte[] endFrame;
        private int leds = 8;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _taskInstance = taskInstance;
            _deferral = _taskInstance.GetDeferral();
            _taskInstance.Canceled += TaskInstance_Canceled;
            var gpio = GpioController.GetDefault();

            // turn off red en green led
            //var red = gpio.OpenPin(35);
            //red.Write(GpioPinValue.Low);
            //red.SetDriveMode(GpioPinDriveMode.Output);
            //var green = gpio.OpenPin(47);
            //green.Write(GpioPinValue.Low);
            //green.SetDriveMode(GpioPinDriveMode.Output);

            // power cycle +3v3 to reset all I2C devices
            //_shdn = gpio.OpenPin(5);
            //_shdn.Write(GpioPinValue.High);
            //_shdn.SetDriveMode(GpioPinDriveMode.Output);
            //_shdn.Write(GpioPinValue.Low);
            //// wait to discharge all capacitors
            //Task.Delay(3000).GetAwaiter().GetResult();
            //_shdn.Write(GpioPinValue.High);

            _heater = gpio.OpenPin(16);
            _heater.Write(GpioPinValue.Low);
            _heater.SetDriveMode(GpioPinDriveMode.Output);

            _clock = new Clock();
            _clock.Start();

            await Init();
            await _clock.Init();

            _bme280 = new BME280();
            await _bme280.Init();

            _timer2 = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick2, TimeSpan.FromMilliseconds(10));
            _timer3 = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick3, TimeSpan.FromMinutes(1));
        }

        private Color HsvToRgb(int h, byte s, byte v)
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

        private async Task Init()
        {
            var deviceSelector = I2cDevice.GetDeviceSelector();
            var controllers = await DeviceInformation.FindAllAsync(deviceSelector);

            _mcp3425 = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(0x68) { BusSpeed = I2cBusSpeed.FastMode });
            _mcp3425.Write(new byte[] { 0x98 }); // 16bit

            _tsl2561 = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(0x39) { BusSpeed = I2cBusSpeed.FastMode });
            _tsl2561.Write(new byte[] { 0x80, 0x03 }); // power on :)

            // spi stuff
            var settings = new SpiConnectionSettings(0)
            {
                ClockFrequency = 10000000,
                Mode = SpiMode.Mode0
            };
            string spiAqs = SpiDevice.GetDeviceSelector("SPI0");
            var deviceInformation = await DeviceInformation.FindAllAsync(spiAqs);
            _spiDevice = await SpiDevice.FromIdAsync(deviceInformation[0].Id, settings);
            endFrame = new byte[(int)Math.Ceiling(leds / 16d)];
            for (var i = 0; i < endFrame.Length; i++)
            {
                endFrame[i] = 0x0;
            }

            var buffer = new byte[4 + (leds * 4) + endFrame.Length];
            for (var i = 0; i < leds; i++)
            {
                Buffer.BlockCopy(new byte[] { 0xE0, 0x0, 0x0, 0x0 }, 0, buffer, 4 + (4 * i), 4);
            }
            Buffer.BlockCopy(endFrame, 0, buffer, 4 + (leds * 4), endFrame.Length);

            _spiDevice.Write(buffer);
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _clock.Dispose();
            _timer2.Cancel();
            _timer3.Cancel();
            _deferral.Complete();
        }

        private void Timer_Tick2(ThreadPoolTimer timer)
        {
            if (_spiDevice != null)
            {
                try
                {
                    var buffer = new byte[4 + (leds * 4) + endFrame.Length];

                    var color = HsvToRgb(_hue1, 255, 255);
                    Buffer.BlockCopy(new byte[] { _brightness, color.B, color.G, color.R }, 0, buffer, 4, 4);
                    color = HsvToRgb(_hue2, 255, 255);
                    Buffer.BlockCopy(new byte[] { _brightness, color.B, color.G, color.R }, 0, buffer, 8, 4);
                    color = HsvToRgb(_hue3, 255, 255);
                    Buffer.BlockCopy(new byte[] { _brightness, color.B, color.G, color.R }, 0, buffer, 12, 4);
                    color = HsvToRgb(_hue4, 255, 255);
                    Buffer.BlockCopy(new byte[] { _brightness, color.B, color.G, color.R }, 0, buffer, 16, 4);
                    color = HsvToRgb(_hue5, 255, 255);
                    Buffer.BlockCopy(new byte[] { _brightness, color.B, color.G, color.R }, 0, buffer, 20, 4);
                    color = HsvToRgb(_hue6, 255, 255);
                    Buffer.BlockCopy(new byte[] { _brightness, color.B, color.G, color.R }, 0, buffer, 24, 4);
                    color = HsvToRgb(_hue7, 255, 255);
                    Buffer.BlockCopy(new byte[] { _brightness, color.B, color.G, color.R }, 0, buffer, 28, 4);
                    color = HsvToRgb(_hue8, 255, 255);
                    Buffer.BlockCopy(new byte[] { _brightness, color.B, color.G, color.R }, 0, buffer, 32, 4);

                    Buffer.BlockCopy(endFrame, 0, buffer, 4 + (leds * 4), endFrame.Length);
                    _spiDevice.Write(buffer);

                    _hue1 += 1;
                    if (_hue1 > 360)
                        _hue1 = 0;
                    _hue2 += 1;
                    if (_hue2 > 360)
                        _hue2 = 0;
                    _hue3 += 1;
                    if (_hue3 > 360)
                        _hue3 = 0;
                    _hue4 += 1;
                    if (_hue4 > 360)
                        _hue4 = 0;
                    _hue5 += 1;
                    if (_hue5 > 360)
                        _hue5 = 0;
                    _hue6 += 1;
                    if (_hue6 > 360)
                        _hue6 = 0;
                    _hue7 += 1;
                    if (_hue7 > 360)
                        _hue7 = 0;
                    _hue8 += 1;
                    if (_hue8 > 360)
                        _hue8 = 0;


                }
                catch (Exception)
                {
                }
            }
        }

        private void Timer_Tick3(ThreadPoolTimer timer)
        {
            var temperature = 0d;
            var pressure = 0d;
            var humidity = 0d;
            var lux = 0;
            var adc = 0d;

            if (_mcp3425 != null)
            {
                var buffer = new byte[3];
                _mcp3425.Read(buffer);
                adc = (256 * buffer[0] + buffer[1]) * 0.0000625;
                Debug.WriteLine("adc: " + adc);
            }

            if (_bme280 != null)
            {
                _bme280.Sample();
                temperature = _bme280.Temperature;
                pressure = _bme280.Pressure;
                humidity = _bme280.Humidity;

                Debug.WriteLine($"temperature: {_bme280.Temperature} DegC");
                Debug.WriteLine($"pressure: {_bme280.Pressure} Pa");
                Debug.WriteLine($"humidity: {_bme280.Humidity} %rH");
            }

            if (_tsl2561 != null)
            {
                var buffer = new byte[2];
                _tsl2561.WriteRead(new byte[] { 0xAC }, buffer);
                lux = 256 * buffer[1] + buffer[0];
                Debug.WriteLine("light: " + lux);
            }

            using (var client = new HttpClient())
            {
                var apikey = "";
                var result = client.GetAsync($"https://api.thingspeak.com/update?api_key={apikey}&field1={temperature}&field2={pressure}&field3={humidity}&field4={lux}&field5={adc}").Result;
            }
        }
    }
}