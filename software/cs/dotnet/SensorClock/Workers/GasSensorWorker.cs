using System.Device.Gpio;

namespace SensorClock.Workers
{
    /// <summary>
    /// Temporary, for now makes sure RPI GPIO pin 16 is LOW, eg. disabling the heater
    /// </summary>
    public class GasSensorWorker : BackgroundService
    {
        private readonly GpioController _controller;
        private readonly ILogger<GasSensorWorker> _logger;

        public GasSensorWorker(ILogger<GasSensorWorker> logger)
        {
            _logger = logger;
            _controller = new GpioController();
            _controller.OpenPin(16, PinMode.Output);
            _controller.Write(16, PinValue.Low);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.Delay(-1, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }

        public override void Dispose()
        {
            _controller.Write(16, PinValue.Low);
            _controller.Dispose();

            base.Dispose();
        }
    }
}
