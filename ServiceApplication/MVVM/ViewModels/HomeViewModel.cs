using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.DependencyInjection;
using ServiceApplication.MVVM.Models;
using ServiceApplication.Services;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ServiceApplication.MVVM.ViewModels
{
   public partial class HomeViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DateAndTimeService _dateAndTimeService;
        private readonly WeatherService _weatherService;
        private readonly IotHubManager _iotHubManager;
        private readonly DeviceService _deviceService;

        public HomeViewModel(IServiceProvider serviceProvider, DateAndTimeService dateAndTimeService,
            WeatherService weatherService, IotHubManager iotHubManager, DeviceService deviceService)
        {
            _serviceProvider = serviceProvider;
            _dateAndTimeService = dateAndTimeService;
            _weatherService = weatherService;
            _iotHubManager = iotHubManager;
            _deviceService = deviceService;         

            UpdateDeviceList();
            UpdateDateAndTime();
            UpdateWeather();
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
        private string? _currentTemperature = "--";

        [ObservableProperty]
        private string? _currentTemperatureUnit = "°C";

        //[ObservableProperty]
        //public ObservableCollection<DeviceItem>? _deviceList;

        [ObservableProperty]
        public ObservableCollection<DeviceItemViewModel>? _deviceList;






        [RelayCommand] // Command i frontend
        private void NavigateToSettings() 
        { //hur navigeringen ska gå till. 
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>(); 
            //behöver serviceprovider för att undvika att nya upp en ny instans
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
            // Lyssnar på en action-förändring. Varje gång action förändras så triggar
            // denna 
            _weatherService.WeatherUpdated += () =>
            {
                CurrentWeatherCondition = _weatherService.CurrentWeatherCondition;
                CurrentTemperature = _weatherService.CurrentTemperature;
            };
        }
        private void UpdateDeviceList()
        {
            // DeviceListUpdated är en action som lyssnar efter en förändring.
            //_deviceService.DeviceListUpdated += () =>
            //{
            //    DeviceList = new ObservableCollection<DeviceItem>(_deviceService.Devices
            //        .Select(device => new DeviceItem()).ToList());
            //};
            //// DeviceListUpdated är en action som lyssnar efter en förändring.
            _deviceService.DeviceListUpdated += () =>
            {
                DeviceList = new ObservableCollection<DeviceItemViewModel>(_deviceService.Devices
                    .Select(deviceItem => new DeviceItemViewModel(deviceItem)).ToList());
            };
        }
    
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        //object sender - för att hämta ut den data som finns för den här knappen, det här
        //objektet. När vi autogenereras ett objekt följer det med metadata som hamnar i
        //objectsender. Vi behöver göra om objektet till en knapp.
        {
            try
            {
                Button? button = sender as Button; //gör om objektet till en knapp
                if (button != null)
                {
                    Twin? twin = button.DataContext as Twin; // datacontext är metadata som är på objektet.
                                                             //Här talar i om att det här objektet som genererats är av typen twin

                    if (twin != null)
                    {
                        string deviceId = twin.DeviceId;

                        if (!string.IsNullOrEmpty(deviceId))
                            await _iotHubManager.SendMethodAsync(new MethodDataRequest
                            {
                                DeviceId = deviceId,
                                MethodName = "start"
                            });
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? button = sender as Button; //gör om objektet till en knapp
                if (button != null)
                {
                    Twin? twin = button.DataContext as Twin; // datacontext är metadata som är på objektet.
                                                             //Här talar i om att det här objektet som genererats är av typen twin

                    if (twin != null)
                    {
                        string deviceId = twin.DeviceId;

                        if (!string.IsNullOrEmpty(deviceId))
                            await _iotHubManager.SendMethodAsync(new MethodDataRequest
                            {
                                DeviceId = deviceId,
                                MethodName = "stop"
                            });
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

    }
}
