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

        private readonly string _connectionString = "";
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
