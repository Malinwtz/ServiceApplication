using DataAccess.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLibrary.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services=>
    {
        services.AddDbContext<ApplicationDbContext>();
        services.AddSingleton<IotHubManager>();
    })
    .Build();

host.Run();
