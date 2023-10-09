﻿using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SharedLibrary.Services
{    
    public class IotHubManager
    {
        private readonly RegistryManager _registryManager; 
        private readonly ServiceClient _serviceClient; 
        private EventHubConsumerClient _consumerClient; 
        private readonly Timer _timer;  //using System.Timers;

        public List<DeviceItem> Devices { get; private set; }

        public event Action? DeviceListUpdated;

        public IotHubManager(IotHubManagerOptions options)
        {
            _registryManager = RegistryManager.CreateFromConnectionString(options.IotHubConnectionString);
            _serviceClient = ServiceClient.CreateFromConnectionString(options.IotHubConnectionString);
            _consumerClient = new EventHubConsumerClient(options.ConsumerGroup, options.EventHubEndPoint);
            
            Devices = new List<DeviceItem>();
            Task.Run(GetAllDevicesAsync);
            _timer = new Timer(5000);
            _timer.Elapsed += async (s, e) => await GetAllDevicesAsync();
            _timer.Start();
        }


        //metod för att skicka vilken metod som registrerats
        public async Task<CloudToDeviceMethodResult> SendMethodAsync(MethodDataRequest req)
        {
            try
            {
                var cloudMethod = new CloudToDeviceMethod(req.MethodName)
                {
                    ConnectionTimeout = new TimeSpan(req.ResponseTimeout)
                };
                if (req.Payload != null)
                    cloudMethod.SetPayloadJson(JsonConvert.SerializeObject(req.Payload));

                var result = await _serviceClient.InvokeDeviceMethodAsync(req.DeviceId, cloudMethod);
                if (result != null)
                    return result;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null!;
        }


        // Metod för att hämta upp alla devices som en lista av jsonobjekt/strängar
        public async Task<IEnumerable<string>> GetDevicesAsJsonAsync()
        {
            try
            {
                var devices = new List<string>();
                //här vill vi hämta devices med en query. Hämta alla. Använder en sql-lik query
                var result = _registryManager.CreateQuery("select * from devices");
                // får en iquery som resultat - en lista som vi vill loopa igenom
                if (result.HasMoreResults)
                    foreach (var device in await result.GetNextAsJsonAsync()) // hämta ut det som ett jsonobjekt
                        devices.Add(device);                       // spara i en lista av strängar                       

                return devices;
            }
            catch (Exception ex) { Debug.WriteLine($"{ex.Message}"); }
            return null!;
        }

        // Metod för att hämta upp alla devices som en lista av Twins
        public async Task<IEnumerable<Twin>> GetDevicesAsTwinAsync(string sqlQuery = "select * from devices")
        {
            try
            {
                var devices = new List<Twin>();
                //här vill vi hämta devices med en query. Hämta alla. Använder en sql-lik query
                var result = _registryManager.CreateQuery(sqlQuery);
                // får en iquery som resultat - en lista som vi vill loopa igenom
                if (result.HasMoreResults)
                    foreach (var device in await result.GetNextAsTwinAsync()) // hämta ut det som ett twinobjekt
                        devices.Add(device);                       // spara i en lista av twins                       

                return devices;
            }
            catch (Exception ex) { Debug.WriteLine($"{ex.Message}"); }
            return null!;
        }

        public async Task GetAllDevicesAsync()
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

                        //try { _device.DeviceType = device.Properties.Reported["deviceType"].ToString(); }
                        //catch (Exception ex)
                        //{
                        //    Debug.WriteLine($"Fel: {ex.Message}");
                        //}

                        try { _device.IsActive = bool.Parse(!string.IsNullOrEmpty(device.Properties.Reported["isActive"].ToString())); }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Fel: {ex.Message}");
                        }

                        Devices.Add(_device); // det kommer in devices med properties som det ska
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
                    DeviceListUpdated!.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
