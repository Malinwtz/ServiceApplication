using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ServiceApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApplication.MVVM.ViewModels
{
   public partial class HomeViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DateAndTimeService _dateAndTimeService;
        private readonly WeatherService _weatherService;

        public HomeViewModel(IServiceProvider serviceProvider, DateAndTimeService dateAndTimeService, WeatherService weatherService)
        {
            _serviceProvider = serviceProvider;
            _dateAndTimeService = dateAndTimeService;
            _weatherService = weatherService;

            UpdateDateAndTime(); // uppdatera tiden när man kör ctor. Om man har while-loop låser man hela programmet.
            // om man kör denna metod som async Task och användet task för att köra metoden i ctor så kommer det att gå åt mycket cpu
            // om man använder task.delay(1000) kommer klockan inte att uppdateras som den ska
            // i dateandtimeservcice: public event Action? TimeUpdated; //använder action för att uppdatera tid och datum

            UpdateWeather(); //hämtar temperaturen
        }

        [ObservableProperty]
        private string? _title = "Home";

        [ObservableProperty]
        private string? _currentTime = "--:--";

        [ObservableProperty]
        private string? _currentDate;

        [ObservableProperty]
        private string? _currentWeatherCondition = "\ue137";

        [ObservableProperty]
        private string? _currentOutsideTemperature = "--";

        [ObservableProperty]
        private string? _currentOutsideTemperatureUnit = "°C";

        [ObservableProperty]
        private string? _currentInsideTemperature = "--";

        [ObservableProperty]
        private string? _currentInsideTemperatureUnit = "°C";


        [RelayCommand] // Command i frontend
        private void NavigateToSettings() //lägger till Command på sluet av metodnamnet
        { //hur navigeringen ska gå till. 
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>(); //behöver serviceprovider för att undvika att nya upp en ny instans
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }

        private void UpdateDateAndTime()
        {
            // Lyssnar på en action-förändring. Varje gång action förändras så triggar denna 
            _dateAndTimeService.TimeUpdated += () =>
            {
                CurrentDate = _dateAndTimeService.CurrentDate;
                CurrentTime = _dateAndTimeService.CurrentTime;
            };
        }

        private void UpdateWeather()
        {
            // Lyssnar på en action-förändring. Varje gång action förändras så triggar denna 
            _weatherService.WeatherUpdated += () =>
            {
                CurrentWeatherCondition = _weatherService.CurrentWeatherCondition;
            //    CurrentOutsideTemperature = _weatherService.CurrentTemperature;
            };
        }

    }
}
