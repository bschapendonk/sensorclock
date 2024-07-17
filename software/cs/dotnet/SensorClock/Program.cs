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
/* nano -w ~/.config/systemd/user/sensorclock.service
 * sudo nano -w /etc/systemd/system/sensorclock.service
 * 
 * sudo systemctl daemon-reload
 * sudo systemctl enable sensorclock.service
 * 
 * sudo systemctl start sensorclock.service
 * sudo systemctl stop sensorclock.service
 * 
 * sudo systemctl status sensorclock.service
 * 
 * sudo nano /etc/systemd/timesyncd.conf
 * sudo systemctl restart systemd-timesyncd.service
 * timedatectl status
 * timedatectl show-timesync
 * timedatectl timesync-status
 */