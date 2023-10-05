using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApplication.Services
{
    public class DeviceService
    {
        // Hämta ut saker från min iot hub 

        private readonly string _connectionString = "HostName=MalinsIotDevice.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=YtL+BJzB3/jZ63a378FyZzHZw2B89lDzOAIoTFDCNP8=";
        private readonly ServiceClient _serviceClient;

        public DeviceService()
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
        }

        //private async Task GetMessagesAsync()
        //{

        //}
    }
}
