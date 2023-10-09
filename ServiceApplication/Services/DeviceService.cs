using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Rest;
using Newtonsoft.Json;
using SharedLibrary.Models;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Documents;

namespace ServiceApplication.Services
{
    public class DeviceService
    {
       
        private readonly string _hubConnectionString = "HostName=MalinsIotDevice.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=YtL+BJzB3/jZ63a378FyZzHZw2B89lDzOAIoTFDCNP8=";


        public DeviceService()
        {
           
        }



        //private async Task GetMessagesAsync()
        //{

        //}
    }
}
