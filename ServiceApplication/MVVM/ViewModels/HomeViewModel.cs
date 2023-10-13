using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ServiceApplication.Services;
using SharedLibrary.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SharedLibrary.Services;

namespace ServiceApplication.MVVM.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DateAndTimeService _dateAndTimeService;
        private readonly WeatherService _weatherService;
        private readonly IotHubManager _iotHubManager;

        public HomeViewModel(IServiceProvider serviceProvider, DateAndTimeService dateAndTimeService,
            WeatherService weatherService, IotHubManager iotHubManager)
        {
            _serviceProvider = serviceProvider;
            _dateAndTimeService = dateAndTimeService;
            _weatherService = weatherService;
            _iotHubManager = iotHubManager;

            UpdateWeather();
            UpdateDeviceList();
            UpdateDateAndTime();
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

        [ObservableProperty]
        public ObservableCollection<DeviceItemViewModel>? _deviceList;

        [RelayCommand]
        public async Task StartStopButton(DeviceItemViewModel device)
        {
            if (device != null)
            {
                var isDeviceActive = device.DeviceItem.IsActive;
                try
                {
                    if (!string.IsNullOrEmpty(device.DeviceItem.DeviceId))
                    {
                        if (isDeviceActive == false)
                        {
                            await _iotHubManager.SendMethodAsync(new MethodDataRequest
                            {
                                DeviceId = device.DeviceItem.DeviceId,
                                MethodName = "start"
                            });
                            isDeviceActive = true;
                        }
                        else
                        {
                            await _iotHubManager.SendMethodAsync(new MethodDataRequest
                            {
                                DeviceId = device.DeviceItem.DeviceId,
                                MethodName = "stop"
                            });
                            isDeviceActive = false;
                        }
                    }
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
            }           
        }

        [RelayCommand]
        private void NavigateToSettings()
        { //hur navigeringen ska gå till. 
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            //behöver serviceprovider för att undvika att nya upp en ny instans
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }
        private void UpdateDateAndTime()
        {
            _dateAndTimeService.TimeUpdated += () =>
            {
                CurrentDate = _dateAndTimeService.CurrentDate;
                CurrentTime = _dateAndTimeService.CurrentTime;
            };
        }
        private void UpdateWeather()
        {
            _weatherService.WeatherUpdated += () =>
            {
                CurrentWeatherCondition = _weatherService.CurrentWeatherCondition; 
                CurrentTemperature = _weatherService.CurrentTemperature;
            };
        }
        private void UpdateDeviceList()
        {
            _iotHubManager.DeviceListUpdated += () =>
            {
                DeviceList = new ObservableCollection<DeviceItemViewModel>(_iotHubManager.Devices
                    .Select(deviceItem => new DeviceItemViewModel(deviceItem)).ToList());
            };
        }
    }
}
