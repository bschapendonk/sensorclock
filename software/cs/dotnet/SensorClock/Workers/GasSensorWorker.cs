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
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _controller.OpenPin(16, PinMode.Output);
            _controller.Write(16, PinValue.Low);

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
            if (_controller.IsPinOpen(16))
                _controller.Write(16, PinValue.Low);
            _controller.Dispose();

            base.Dispose();
        }
    }
}
