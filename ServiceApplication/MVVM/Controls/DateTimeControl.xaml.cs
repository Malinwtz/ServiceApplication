using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ServiceApplication.MVVM.Controls
{

    // INotifyPropertyChanged är ett interface som lyssnar (i bakgrunden) på förändringar.
    // Detta gör att frontend kan ändras, att det grafiska gränssnittet kan ändras utan
    // att låsa upp tråden. 
    public partial class DateTimeControl : UserControl, INotifyPropertyChanged
    {

        // De publika properties måste mellanlagras i privata variabler. 
        private string? _currentTime;
        private string? _currentDate;

        public string? CurrentTime
        {
            get => _currentTime;
            set { _currentTime = value; OnPropertyChanged(nameof(CurrentTime)); }
        }
        public string? CurrentDate
        {
            get => _currentDate;
            set { _currentDate = value; OnPropertyChanged(nameof(CurrentDate)); }
        }


        // Denna implementeras från interfacet. 
        public event PropertyChangedEventHandler? PropertyChanged;
        // Metod som lyssnar på förändringar.
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // Konstruktor
        public DateTimeControl()
        {
            InitializeComponent();
            DataContext = this;

            // Här kör vi metoden som set:ar tid och datum så kommer vi att få tid och datum i vyn. 
            SetDateTime();

            // Lägg till ett namespace.
            DispatcherTimer timer = new DispatcherTimer
            {
                // Här säger vi att klockan ska uppdateras varje sekund. 
                Interval = TimeSpan.FromSeconds(1),
            };
            // Här set:ar vi tid och datum
            timer.Tick += (s, e) => SetDateTime();
            timer.Start();
        }


        // Metod för att sätta datum och tid. 
        private void SetDateTime()
        {
            CurrentTime = DateTime.Now.ToString("HH:mm");
            // dddd: Dagens namn, d: Datumet, MMMM: Hela månaden, yyyy: Hela årtalet
            CurrentDate = DateTime.Now.ToString("dddd, d MMMM yyyy");
        }
    }
}
