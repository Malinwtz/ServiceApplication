using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks; // timer
using System.Timers;

namespace ServiceApplication.Services
{
    public class WeatherService
    {
      //  private readonly string _url = "https://api.openweathermap.org/data/2.5/weather?lat=59.1875&lon=18.1232&appid=b4a3119e986341f8f3a4d159c5787679";
        private readonly string _url = "https://api.openweathermap.org/data/2.5/weather?lat=59.32944&lon=18.06861&appid=4f325800efc5078dcc527859127930bb";
        // api key: 4f325800efc5078dcc527859127930bb
        // private readonly string _url = "http://api.openweathermap.org/data/2.5/weather?q=Stockholm&appid=4f325800efc5078dcc527859127930bb&units=metric";
        private readonly Timer _timer;  
        private readonly HttpClient _http; //vill nå ett api med httpclient
        public string? CurrentWeatherCondition { get; private set; }
        public string? CurrentTemperature { get; private set; }
        public event Action? WeatherUpdated;

        public WeatherService(HttpClient client)
        {
            _http = client;
            Task.Run(SetCurrentWeatherAsync);
            _timer = new Timer(60000 * 15);      
            _timer.Elapsed += async (s, e) => await SetCurrentWeatherAsync();
            _timer.Start();
        }

        private async Task SetCurrentWeatherAsync()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(await _http.GetStringAsync(_url));                                                                                                                  
                CurrentTemperature = (data!.main.temp - 273.15).ToString("#");
                CurrentWeatherCondition = GetWeatherConditionIcon(data!.weather[0].description.ToString()); 
                var weatherDescription = (data!.weather[0].icon); // hämtar apiets egen kod för väderikon. 
            }
            catch
            {
                CurrentTemperature = "--";
                CurrentWeatherCondition = GetWeatherConditionIcon("--");
            }
            WeatherUpdated?.Invoke();   
        }

        private string GetWeatherConditionIcon(string value)
        {
            return value switch
            {
                "clear sky" => "\ue28f",
                "few clouds" => "\uf6c4",
                "overcast clouds" => "\uf744",
                "scattered clouds" => "\uf0c2",
                "broken clouds" => "\uf744",
                "shower rain" => "\uf738",
                "rain" => "\uf740",
                "thunderstorm" => "\uf76c",
                "snow" => "\uf742",
                "mist" => "\uf74e",
                _ => "\ue137",
            };
        }       
    }
}
