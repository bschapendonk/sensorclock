using SensorClock.Workers;

var host = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .ConfigureServices(services =>
    {
        services.AddHostedService<GasSensorWorker>();
        services.AddHostedService<Apa102Worker>();
        services.AddHostedService<ClockWorker>();
    })
    .Build();

await host.RunAsync();

// https://blog.maartenballiauw.be/post/2021/05/25/running-a-net-application-as-a-service-on-linux-with-systemd.html
