using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace dotnet
{
    public class GasSensorWorker : BackgroundService
    {
        private GpioController _controller;
        private readonly ILogger<GasSensorWorker> _logger;

        public GasSensorWorker(ILogger<GasSensorWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Init();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void Init()
        {
            _controller = new GpioController(PinNumberingScheme.Board);
            _controller.OpenPin(16, PinMode.Output);
            _controller.Write(16, PinValue.Low);
        }

        public override void Dispose()
        {
            _controller?.Write(16, PinValue.Low);
            _controller?.Dispose();

            base.Dispose();
        }
    }
}
