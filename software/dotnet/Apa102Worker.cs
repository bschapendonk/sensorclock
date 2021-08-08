using Iot.Device.Apa102;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Device.Spi;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet
{
    /// <summary>
    /// Temporary, for now makes sure all APA102 are off
    /// </summary>
    public class Apa102Worker : BackgroundService
    {
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
