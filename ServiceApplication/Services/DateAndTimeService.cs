using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServiceApplication.Services
{
    public class DateAndTimeService
    {
        private readonly Timer _timer; //vill använda system.Timers
        public event Action? TimeUpdated; //använder action för att uppdatera tid och datum
        public string? CurrentDate { get; private set; }
        public string? CurrentTime { get; private set; }
        public DateAndTimeService()
        {
            SetCurrentDateAndTime();

            _timer = new Timer(1000); // nu behövs inte en while-loop
            _timer.Elapsed += (s, e) => SetCurrentDateAndTime();
            _timer.Start();
        }



        private void SetCurrentDateAndTime()
        {
            //behöver inte while-loop för vi har en timer. 

            CurrentTime = DateTime.Now.ToString("HH:mm");
            CurrentDate = DateTime.Now.ToString("dddd, d MMMM yyyy");

            TimeUpdated?.Invoke();  //lägg till för att uppdatera tiden
        }
    }
}
