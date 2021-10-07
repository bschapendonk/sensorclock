using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Iot.Device.Bmxx80;
using IoT.Device.Tsl256x;
using System.Device.I2c;
using UnitsNet;

namespace SensorClock.Workers
{
    public class InfluxDBWorker : BackgroundService
    {
        private readonly Bme280 _bme280;
        private readonly Tsl256x _tsl2561;
        private readonly InfluxDBClient _client;
        private readonly ILogger<Apa102Worker> _logger;

        public InfluxDBWorker(ILogger<Apa102Worker> logger, IConfiguration configuration)
        {
            _logger = logger;

            _client = InfluxDBClientFactory.Create("http://localhost:8086", "sensorclock", "sensorclock".ToArray());

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
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var write = _client.GetWriteApiAsync();

                var result = await _bme280.ReadAsync();
                if (result.Humidity.HasValue)
                {
                    await write.WritePointAsync("sensorclock","sensorclock",
                        PointData.Measurement("humidity").Field("value", result.Humidity.Value.Value).Timestamp(DateTime.UtcNow, WritePrecision.S),
                        stoppingToken);
                }

                if (result.Pressure.HasValue)
                {
                    await write.WritePointAsync("sensorclock", "sensorclock",
                        PointData.Measurement("pressure").Field("value", result.Pressure.Value.Value).Timestamp(DateTime.UtcNow, WritePrecision.S),
                        stoppingToken);
                }

                if (result.Temperature.HasValue)
                {
                    await write.WritePointAsync("sensorclock", "sensorclock",
                        PointData.Measurement("temperature").Field("value", result.Temperature.Value.Value).Timestamp(DateTime.UtcNow, WritePrecision.S),
                        stoppingToken);
                }

                await write.WritePointAsync("sensorclock", "sensorclock",
                        PointData.Measurement("lux").Field("value", _tsl2561.MeasureAndGetIlluminance().Value).Timestamp(DateTime.UtcNow, WritePrecision.S),
                        stoppingToken);

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
            _bme280.Dispose();
            _tsl2561.Dispose();
            _client.Dispose();

            base.Dispose();
        }
    }
}
