using ConnectDeviceToHubFunction.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((config, services)=>
    {
        services.AddSingleton(new AzureFunctionIotHubManager(config.Configuration.GetConnectionString("IotHub")!));
    })
    .Build();

host.Run();
