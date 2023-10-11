using ConnectDeviceToHubFunction.Services;
using DataAccess.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLibrary.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((config, services)=>
    {
        services.AddSingleton(new AzureFunctionIotHubManager(config.Configuration.GetConnectionString("IotHub")!));
    })
    .Build();

host.Run();
