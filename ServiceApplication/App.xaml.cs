using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceApplication.MVVM.ViewModels;
using ServiceApplication.Services;
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
                .ConfigureServices((config, services) =>
                {
                    services.AddTransient<HttpClient>();
                    services.AddSingleton<DateAndTimeService>();
                    services.AddSingleton<WeatherService>();
                    //services.AddSingleton<MainWindowViewModel>(); //
                    //services.AddSingleton<DeviceService>();
                    //services.AddSingleton<NavigationStore>(); //signleton eftersom instansen bara lever på den här applikationen. Sen vill vi ha kvar samma info över flera vyer så den inte gör en ny insdtans varje gång 

                    services.AddSingleton<ApplicationService>(); //servicen för att stänga ner programmet lägger vi till här                 
                    services.AddSingleton<HomeViewModel>();
                    ////Lägg till viewmodels så att de kan hämtas
                    services.AddSingleton<SettingsViewModel>(); //vi vill ha samma settingsfönster varje gång vi använder den
                    services.AddSingleton<MainWindow>(); //så att vi kan köra applikationen
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs args) //starta upp våra services
        {
            await AppHost!.StartAsync(); //startar servern

            var mainWindow = AppHost!.Services.GetRequiredService<MainWindow>();

            //vilken är min currentviewmodel? vi måste välja vilken viewmodel som ska startas upp
            //var navigationStore = AppHost!.Services.GetRequiredService<NavigationStore>(); //hämta min navigationstore 
            //navigationStore.CurrentViewModel = new HomeViewModel(navigationStore, ); //lägg till en ny instans av homeviewmodel, det är denna motsvarande vy som ska laddas in. Bygger på att viewen ärver observableobjects

            mainWindow.Show();

            base.OnStartup(args);
        }


    }
}
