using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
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


    // D12 devicemanager getalldevices
    public class DeviceService
    {
        // Hämta ut saker från min iot hub 

        private readonly string _connectionString = "HostName=MalinsIotDevice.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=YtL+BJzB3/jZ63a378FyZzHZw2B89lDzOAIoTFDCNP8=";
        private readonly ServiceClient _serviceClient;
        private readonly RegistryManager _registryManager;
        private readonly Timer _timer;  //using System.Timers;
        private readonly IotHubManager _iotHubManager;
        public List<DeviceItem> Devices { get; private set; } 

        public event Action DeviceListUpdated;


        public DeviceService(IotHubManager iotHubManager)
        {
            _iotHubManager = iotHubManager; 
            _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
            _serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
                 
            Devices = new List<DeviceItem>();

            Task.Run(GetAllDevicesAsync);

            _timer = new Timer(5000);
            _timer.Elapsed += async (s, e) => await GetAllDevicesAsync();
            _timer.Start();
        }

        //private async Task GetDeviceTwinAsync()
        //{
        //    try
        //    {
        //        DeviceTwinList.Clear();
        //        var twinList = await _iotHubManager.GetDevicesAsTwinAsync();
        //        foreach (var twin in twinList)
        //            DeviceTwinList.Add(twin);

        //        DeviceListUpdated?.Invoke();
  
        //    }
        //    catch (Exception ex) { Debug.WriteLine(ex.Message); }
        //}

        private async Task GetAllDevicesAsync()
        {
            try
            {
                var updated = false;
                var list = new List<Twin>();
                var result = _registryManager.CreateQuery("select * from devices");

                foreach (var item in await result.GetNextAsTwinAsync())
                    list.Add(item);

                foreach (var device in list)
                    if (!Devices.Any(x => x.DeviceId == device.DeviceId))
                    {
                        var _device = new DeviceItem { DeviceId = device.DeviceId };

                        try { _device.DeviceType = device.Properties.Reported["deviceType"].ToString(); }
                        catch { }

                        try { _device.IsActive = bool.Parse(!string.IsNullOrEmpty(device.Properties.Reported["isActive"].ToString())); }
                        catch { }

                        Devices.Add(_device);
                        updated = true;
                    }
                for (int i = Devices.Count - 1; i >= 0; i--)
                {
                    if (!list.Any(x => x.DeviceId == Devices[i].DeviceId))
                    {
                        Devices.RemoveAt(i);
                        updated = true;
                    }
                }
                if (updated)
                    DeviceListUpdated.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }


        //private async Task GetMessagesAsync()
        //{

        //}
    }
}
