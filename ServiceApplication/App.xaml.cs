﻿using DataAccess.Contexts;
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
                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //})
                .ConfigureServices((config, services) =>
                {
                    services.AddSingleton(new IotHubManager(new IotHubManagerOptions()));   
                    services.AddDbContext<ApplicationDbContext>(
                        x => x.UseSqlite($"Data Source=Database.sqlite.db", 
                        x=> x.MigrationsAssembly(nameof(DataAccess))));
                    services.AddTransient<HttpClient>();
                    services.AddSingleton<DateAndTimeService>();
                    services.AddSingleton<WeatherService>();
                    services.AddSingleton<MainWindowViewModel>(); 
                    services.AddSingleton<DeviceService>();
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


//services.AddSingleton(new IotHubManager(new IotHubManagerOptions(
//    {
//    IotHubConnectionString = "HostName=MalinsIotDevice.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=YtL+BJzB3/jZ63a378FyZzHZw2B89lDzOAIoTFDCNP8=",
//    EventHubEndPoint = "Endpoint=sb://ihsuprodamres049dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=YtL+BJzB3/jZ63a378FyZzHZw2B89lDzOAIoTFDCNP8=;EntityPath=iothub-ehub-malinsiotd-25231991-006434fe96",
//    EventHubName = "iothub-ehub-malinsiotd-25231991-006434fe96",
//    ConsumerGroup = "serviceapplication"
//}));
