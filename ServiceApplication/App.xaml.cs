using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceApplication.MVVM.ViewModels;
using ServiceApplication.Services;
using SharedLibrary.Models;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceApplication
{
    public partial class App : Application
    {
        private static IHost? AppHost { get; set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((config, services) =>
                {
                    services.AddSingleton<IotHubManager>();
                    services.AddSingleton(new IotHubManagerOptions
                    {
                        IotHubConnectionString = config.Configuration.GetConnectionString("IotHub")!,
                        EventHubEndPoint = config.Configuration.GetConnectionString("IotHubEndpoint")!,
                        EventHubName = config.Configuration.GetConnectionString("EventHubName")!,
                        ConsumerGroup = "serviceapplication",
                    });

                    //services.AddSingleton(new IotHubManager(new IotHubManagerOptions
                    //{
                    //    IotHubConnectionString = config.Configuration.GetConnectionString("IotHub")!,
                    //    EventHubEndPoint = config.Configuration.GetConnectionString("IotHubEndpoint")!,
                    //    EventHubName = config.Configuration.GetConnectionString("EventHubName")!,
                    //    ConsumerGroup = "serviceapplication",
                    //}));          
                    
                    services.AddDbContext<ApplicationDbContext>(
                        x => x.UseSqlite($"Data Source=Database.sqlite.db", 
                        x=> x.MigrationsAssembly(nameof(DataAccess))));
                    services.AddTransient<HttpClient>();
                    services.AddSingleton<DateAndTimeService>();
                    services.AddSingleton<WeatherService>();
                    services.AddSingleton<MainWindowViewModel>(); 
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<SettingsViewModel>(); 
                    services.AddSingleton<MainWindow>(); 
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs args)
        {
            await AppHost!.StartAsync(); 
            var mainWindow = AppHost!.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(args);
        }
    }
}
