using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services=>
    {
        services.AddSingleton<IotHubManager>();
    })
    .Build();

host.Run();
