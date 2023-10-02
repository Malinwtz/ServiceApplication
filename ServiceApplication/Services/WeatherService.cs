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
        private readonly string _url = "https://api.weathermap.org/data/2.5/weather?lat";
        private readonly Timer _timer;  //using System.Timers;
        private readonly HttpClient _http; //vill nå ett api med httpclient
        public string? CurrentWeatherCondition { get; private set; }
        public string? CurrentOutsideTemperature { get; private set; }
        public string? CurrentInsideTemperature { get; private set; }
        public event Action? WeatherUpdated;


        public WeatherService(HttpClient client)
        {
            _http = client;

            //starta timer
            Task.Run(SetCurrentWeatherAsync);

            _timer = new Timer(60000 * 15);
            _http = new HttpClient(); // när weatherservice skapas ska det skapas en httpclient just för den aööts behöver vi inte använda di för just den. 

            //bygga timer
            _timer.Elapsed += async (s, e) => await SetCurrentWeatherAsync();
            _timer.Start();
        }


        //metod som är async för att vi ska hämta från api
        private async Task SetCurrentWeatherAsync()
        {
            try
            {
                //hämta info från url
                var data = JsonConvert.DeserializeObject<dynamic>(await _http.GetStringAsync(_url)); //gör om till ett dynamiskt objekt
                                                                                                     //  CurrentTemperature = data!.main.temp - 273.15.ToString("#");
                CurrentWeatherCondition = GetWeatherConditionIcon(data!.weather[0].description.ToString());
            }
            catch
            {
               // CurrentTemperature = "--";
                CurrentWeatherCondition = GetWeatherConditionIcon("--");
            }

            WeatherUpdated?.Invoke();   //trigga igång/kör/run
        }

        private string GetWeatherConditionIcon(string value)
        {
            //förenklad switch
            return value switch
            {
                "clear sky" => "\ue28f",
                "few clouds" => "\uf6c4",
                _ => "ue137"
            };
        }

        private async Task GetInsideTemperature()
        {
            try
            {
                await Task.Delay(1000);
                //     var data = JsonConvert.DeserializeObject<dynamic>(await _http.GetStringAsync(_insideUrl));
                //     CurrentInsideTemperature = data!.temperature.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                CurrentInsideTemperature = "--";
            }
        }
    }
}
