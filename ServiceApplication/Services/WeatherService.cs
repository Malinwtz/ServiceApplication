using System;
using System.Collections.Generic;
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
          //  Task.Run(SetCurrentWeatherAsync);

            _timer = new Timer(60000 * 15);
            //_http = new HttpClient(); // när weatherservice skapas ska det skapas en httpclient just för den aööts behöver vi inte använda di för just den. 

            //bygga timer
        //    _timer.Elapsed += async (s, e) => await SetCurrentWeatherAsync();
            _timer.Start();
        }

    }
}
