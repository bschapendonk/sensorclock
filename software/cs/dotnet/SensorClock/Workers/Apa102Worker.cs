using Iot.Device.Apa102;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Device.Spi;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace SensorClock.Workers
{
    /// <summary>
    /// Temporary, for now makes sure all APA102 are off
    /// </summary>
    public class Apa102Worker : BackgroundService
    {
        private int _hue1 = 0;
        private int _hue2 = 45;
        private int _hue3 = 90;
        private int _hue4 = 135;
        private int _hue5 = 180;
        private int _hue6 = 225;
        private int _hue7 = 270;
        private int _hue8 = 315;

        private SpiDevice _spiDevice;
        private Apa102 _apa102;
        private readonly ILogger<Apa102Worker> _logger;

        public Apa102Worker(ILogger<Apa102Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Init();
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    Rainbow();
            //    await Task.Delay(5, stoppingToken);
            //}
            await Task.Delay(-1, stoppingToken);
        }

        private void Init()
        {
            _spiDevice = SpiDevice.Create(new SpiConnectionSettings(0, 0)
            {
                ClockFrequency = 20_000_000,
                DataFlow = DataFlow.MsbFirst,
                Mode = SpiMode.Mode0 // ensure data is ready at clock rising edge
            });
            _apa102 = new Apa102(_spiDevice, 8);
            _apa102.Pixels.Fill(Color.Black);
            _apa102.Flush();
        }

        private void Rainbow()
        {
            _apa102.Pixels[0] = HsvToRgb(_hue1, 255, 255);
            _apa102.Pixels[1] = HsvToRgb(_hue2, 255, 255);
            _apa102.Pixels[2] = HsvToRgb(_hue3, 255, 255);
            _apa102.Pixels[3] = HsvToRgb(_hue4, 255, 255);
            _apa102.Pixels[4] = HsvToRgb(_hue5, 255, 255);
            _apa102.Pixels[5] = HsvToRgb(_hue6, 255, 255);
            _apa102.Pixels[6] = HsvToRgb(_hue7, 255, 255);
            _apa102.Pixels[7] = HsvToRgb(_hue8, 255, 255);
            _apa102.Flush();

            _hue1 += 1;
            if (_hue1 >= 360)
                _hue1 = 0;
            _hue2 += 1;
            if (_hue2 >= 360)
                _hue2 = 0;
            _hue3 += 1;
            if (_hue3 >= 360)
                _hue3 = 0;
            _hue4 += 1;
            if (_hue4 >= 360)
                _hue4 = 0;
            _hue5 += 1;
            if (_hue5 >= 360)
                _hue5 = 0;
            _hue6 += 1;
            if (_hue6 >= 360)
                _hue6 = 0;
            _hue7 += 1;
            if (_hue7 >= 360)
                _hue7 = 0;
            _hue8 += 1;
            if (_hue8 >= 360)
                _hue8 = 0;
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
                case 0: r = v; g = (byte)t; b = (byte)p; break;
                case 1: r = (byte)q; g = v; b = (byte)p; break;
                case 2: r = (byte)p; g = v; b = (byte)t; break;
                case 3: r = (byte)p; g = (byte)q; b = v; break;
                case 4: r = (byte)t; g = (byte)p; b = v; break;
                case 5: r = v; g = (byte)p; b = (byte)q; break;
            }

            return Color.FromArgb(255, r, g, b);
        }

        public override void Dispose()
        {
            _apa102?.Pixels.Fill(Color.Black);
            _apa102?.Flush();
            _apa102?.Dispose();

            _spiDevice?.Dispose();

            base.Dispose();
        }
    }
}