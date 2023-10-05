using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServiceApplication.Services
{
    public class WeatherService
    {
        private readonly string _url = "https://api.openweathermap.org/data/2.5/weather?lat=59.1875&lon=18.1232&appid=b4a3119e986341f8f3a4d159c5787679";
        private readonly Timer _timer;  //using System.Timers;
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


        //metod som är async för att vi ska hämta från api
        private async Task SetCurrentWeatherAsync()
        {
            try
            {
                // Hämta info från url och gör om till ett dynamiskt objekt    
                var data = JsonConvert.DeserializeObject<dynamic>(await _http.GetStringAsync(_url)); 
                                                                                                                 
                CurrentTemperature = (data!.main.temp - 273.15).ToString("#");
                CurrentWeatherCondition = GetWeatherConditionIcon(data!.weather[0].description.ToString());
            }
            catch
            {
                CurrentTemperature = "--";
                CurrentWeatherCondition = GetWeatherConditionIcon("--");
            }

            WeatherUpdated?.Invoke();   //trigga igång/kör/run
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

        //private async Task GetInsideTemperature()
        //{ // För att hämta temperatur från vårt eget api. 
        //    try
        //    {
        //        await Task.Delay(2000);
        //        var data = JsonConvert.DeserializeObject<dynamic>(await _http.GetStringAsync(_url));
        //        CurrentTemperature = data!.temperature.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        CurrentTemperature = "--";
        //    }
        //}
    }
}
