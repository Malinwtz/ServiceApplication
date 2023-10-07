using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using ServiceApplication.MVVM.ViewModels;
using ServiceApplication.Services;
using SharedLibrary.Models;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly IotHubManager? _iotHubManager;
        public ObservableCollection<Twin> DeviceTwinList { get; set; }
          = new ObservableCollection<Twin>();
        // En typ av lista som kan associeras till ListView i frontend

        public ObservableCollection<DeviceItem> Devices { get; set; } =
           new ObservableCollection<DeviceItem>()
       { 
            // Hårdkodad lista med devices som nedan kopplas till frontend för att loopas ut i vyn. 
            new DeviceItem { DeviceId = "1", DeviceType = "light", Placement = "Kitchen", IsActive = true },
            new DeviceItem { DeviceId = "2", DeviceType = "ledstrip", Placement = "Livingroom", IsActive = false },
            new DeviceItem { DeviceId = "3", DeviceType = "fan", Placement = "Bedroom", IsActive = false },
       };

        public HomeView() //IotHubManager iotHubManager) går inte att sätta in iothubmanager här
        {
            InitializeComponent();

            // Denna mappning behövs för att mappa frontend till backend för att listan ska
            // kunna visas. this.DataContext = this; - gör att navigeringen inte fungerar
            // Kopplar listan Devices till frontendlistan


      
           // _iotHubManager = iotHubManager;

            // DeviceList.ItemsSource = DeviceTwinList;
            // Här sätter vi backendlistan till frontendlistan

            // Starta igång metoden för att hämta enheterna till en lista.
            // Starta igång en enhet och servicen. Nu ska vi få upp informationen om min device
           // Task.FromResult(GetDeviceTwinAsync());
        }



        private async Task GetDeviceTwinAsync()
        {
            try
            {
                while (true)
                {
                    var twinList = await _iotHubManager!.GetDevicesAsTwinAsync();
                    //var device = list.FirstOrDefault(); //vi behöver komma åt FirstOrDefault vilket vi gör genom att awaita listan
                    //device.DeviceId //behöver inte jsondelen utan man kan hämta ut direkt från objektet. 
                    //DeviceInformation.Text = device;

                    DeviceTwinList.Clear(); //tömmer listan så att vi bara visar devices en gång i vyn

                    foreach (var twin in twinList) //nu har vi en lista med twins som vi kan gå igenom 
                        DeviceTwinList.Add(twin);  // lägga in våra deviceTwins i en lista

                    await Task.Delay(2000);
                }

            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
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
                            await _iotHubManager!.SendMethodAsync(new MethodDataRequest
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
                            await _iotHubManager!.SendMethodAsync(new MethodDataRequest
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
