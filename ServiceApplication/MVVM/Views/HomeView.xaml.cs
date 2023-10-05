using Microsoft.Azure.Devices;
using ServiceApplication.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceApplication.MVVM.Views
{
 
    public partial class HomeView : UserControl
    {
        public ObservableCollection<DeviceItem> Devices { get; set; } =
           new ObservableCollection<DeviceItem>()
       { 
            // Hårdkodad lista med devices som nedan kopplas till frontend för att loopas ut i vyn. 
            new DeviceItem { DeviceId = "1", DeviceType = "light", Placement = "Kitchen", IsActive = true },
            new DeviceItem { DeviceId = "2", DeviceType = "ledstrip", Placement = "Livingroom", IsActive = false },
            new DeviceItem { DeviceId = "3", DeviceType = "fan", Placement = "Bedroom", IsActive = false },
       };
        public HomeView()
        {
            InitializeComponent();

            // Denna mappning behövs för att mappa frontend till backend för att listan ska
            // kunna visas. this.DataContext = this; - gör att navigeringen inte fungerar
            // Kopplar listan Devices till frontendlistan
            DeviceList.ItemsSource = Devices;
        }
    }
}
