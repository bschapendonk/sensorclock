using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Iot.Device.Bmxx80;
using IoT.Device.Tsl256x;
using System.Device.I2c;

namespace SensorClock.Workers
{
    public class SensorWorker : BackgroundService
    {
        private readonly Bme280 _bme280;
        private readonly Tsl256x _tsl2561;
        private readonly InfluxDBClient _client;
        private readonly ILogger<Apa102Worker> _logger;

        public SensorWorker(ILogger<Apa102Worker> logger, IConfiguration configuration)
        {
            _logger = logger;

            _bme280 = new Bme280(I2cDevice.Create(new I2cConnectionSettings(1, Bmx280Base.DefaultI2cAddress)))
            {
                HumiditySampling = Sampling.Standard,
                PressureSampling = Sampling.Standard,
                TemperatureSampling = Sampling.Standard
            };


            _tsl2561 = new Tsl256x(I2cDevice.Create(new I2cConnectionSettings(1, Tsl256x.DefaultI2cAddress)), PackageType.Other)
            {
                IntegrationTime = IntegrationTime.Integration402Milliseconds,
                Gain = Gain.Normal
            };

            // export CONNECTIONSTRINGS__INFLUXDB="http://localhost:8086?org=&bucket=&token="
            _client = InfluxDBClientFactory.Create(configuration.GetConnectionString("InfluxDB"));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _bme280.ReadAsync();
                var write = _client.GetWriteApiAsync();

                if (result.Humidity.HasValue)
                    await write.WriteMeasurementAsync(WritePrecision.S, result.Humidity.Value, stoppingToken);

                if (result.Pressure.HasValue)
                    await write.WriteMeasurementAsync(WritePrecision.S, result.Pressure.Value, stoppingToken);

                if (result.Temperature.HasValue)
                    await write.WriteMeasurementAsync(WritePrecision.S, result.Temperature.Value, stoppingToken);

                await write.WriteMeasurementAsync(WritePrecision.S, _tsl2561.MeasureAndGetIlluminance(), stoppingToken);

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        public override void Dispose()
        {
            _client.Dispose();
            _bme280.Dispose();
            _tsl2561.Dispose();

            base.Dispose();
        }
    }
}
