using Microsoft.Azure.Devices.Shared;
using SharedLibrary.Models;
using SharedLibrary.Services;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ServiceApplication.MVVM.Controls
{

    public partial class DeviceTileControl : UserControl
    {
        public DeviceTileControl()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //try
            //{
            //    Button? button = sender as Button; //gör om objektet till en knapp
            //    if (button != null)
            //    {
            //        Twin? twin = button.DataContext as Twin; // datacontext är metadata som är på objektet.
            //                                                 //Här talar i om att det här objektet som genererats är av typen twin
            //        if (twin != null)
            //        {
            //            string deviceId = twin.DeviceId;
            //            if (!string.IsNullOrEmpty(deviceId))
            //            {
            //                var device = _iotHubManager.Devices.FirstOrDefault(x => x.DeviceId == deviceId);

            //                if (device != null)
            //                {
            //                    if (device.IsActive == true)
            //                    {
            //                        await _iotHubManager.SendMethodAsync(new MethodDataRequest
            //                        {
            //                            DeviceId = deviceId,
            //                            MethodName = "stop"
            //                        });
            //                    }
            //                    else
            //                    {
            //                        await _iotHubManager.SendMethodAsync(new MethodDataRequest
            //                        {
            //                            DeviceId = deviceId,
            //                            MethodName = "stop"
            //                        });
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { Debug.WriteLine(ex.Message); }

        }
    }
}

