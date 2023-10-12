using Azure.Messaging.EventHubs.Consumer;
using DataAccess.Contexts;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.EntityFrameworkCore;
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
        private string _connectionString = string.Empty;
      //  private bool isConfigured;
        private RegistryManager _registryManager;
        private ServiceClient _serviceClient;
    //    private EventHubConsumerClient _consumerClient;
        private readonly Timer _timer;  //using System.Timers;
        public List<DeviceItem> Devices { get; private set; }
        public event Action? DeviceListUpdated;

      //  private readonly ApplicationDbContext _context;

        public IotHubManager(IotHubManagerOptions options) //, ApplicationDbContext context) // med denna visas inte vädret
        {
           // _context = context;
            _connectionString = options.IotHubConnectionString;

            _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
            _serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
            //isConfigured = true;

            Devices = new List<DeviceItem>();
            Task.Run(GetAllDevicesAsync);
            _timer = new Timer(2000);
            _timer.Elapsed += async (s, e) => await GetAllDevicesAsync();
            _timer.Start();
        }

        //public void IsConfigured() // ta bort - test
        //{
        //    if (!string.IsNullOrEmpty(_connectionString))
        //    {
        //        _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
        //        _serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
        //        isConfigured = true;
        //    }
        ////}
        //public void Initialize(string connectionString = null!) // default = null
        //{
        //    try
        //    {
        //        _connectionString = connectionString;

        //        if (!isConfigured)
        //        {
        //            if (!string.IsNullOrEmpty(_connectionString))
        //            {
        //                _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
        //                _serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
        //                isConfigured = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex) { Debug.Write(ex.Message); }
        //}
        //public async Task InitializeAsync(string connectionString = null!) // default = null
        //{
        //    try
        //    { 
        //        if (!isConfigured) //om programmet inte är konfigurerat körs koden
        //        {
        //            if (string.IsNullOrEmpty(_connectionString)) // Finns INGEN connstring kollar vi efter en i databasen 
        //            {
        //                var settings = await _context.Settings.FirstOrDefaultAsync(); //  > settings blir null ... 
        //                if (settings != null)
        //                {
        //                    _registryManager = RegistryManager.CreateFromConnectionString(settings.ConnectionString);
        //                    _serviceClient = ServiceClient.CreateFromConnectionString(settings.ConnectionString);
        //                    isConfigured = true;
        //                }
        //                else
        //                {
        //                    // ta bort else - lagt till för att testa
        //                    _registryManager = RegistryManager.CreateFromConnectionString(connectionString);
        //                    _serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
        //                }
        //            }
        //            else
        //            {
        //                _registryManager = RegistryManager.CreateFromConnectionString(connectionString);
        //                _serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
        //                isConfigured = true;
        //            }
        //        }              
        //    }
        //    catch (Exception ex) { Debug.Write(ex.Message); }
        //}

        //metod för att skicka vilken metod som registrerats
        public async Task<CloudToDeviceMethodResult> SendMethodAsync(MethodDataRequest req)
        {
            try
            {
                var cloudMethod = new CloudToDeviceMethod(req.MethodName)
                {
                    ConnectionTimeout = new TimeSpan(req.ResponseTimeout)
                };
                if (req.Payload != null) cloudMethod.SetPayloadJson(JsonConvert.SerializeObject(req.Payload));

                var result = await _serviceClient.InvokeDeviceMethodAsync(req.DeviceId, cloudMethod);
                if (result != null) return result;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return null!;
        }
        //public async Task<IEnumerable<string>> GetDevicesAsJsonAsync()
        //{
        //    try
        //    {
        //        var devices = new List<string>();
        //        //här vill vi hämta devices med en query. Hämta alla. Använder en sql-lik query
        //        var result = _registryManager.CreateQuery("select * from devices");
        //        // får en iquery som resultat - en lista som vi vill loopa igenom
        //        if (result.HasMoreResults)
        //            foreach (var device in await result.GetNextAsJsonAsync()) // hämta ut det som ett jsonobjekt
        //                devices.Add(device);                       // spara i en lista av strängar                       

        //        return devices;
        //    }
        //    catch (Exception ex) { Debug.WriteLine($"{ex.Message}"); }
        //    return null!;
        //}  // Metod för att hämta upp alla devices som en lista av jsonobjekt/strängar
        //public async Task<IEnumerable<DeviceItem>> GetDeviceAsJsonAsync()
        //{
        //    try
        //    {
        //        var devices = new List<DeviceItem>();
        //        var result = _registryManager.CreateQuery("select * from devices");
        //        if (result.HasMoreResults)
        //        {
        //            var jsonSerializer = new JsonSerializer();
        //            foreach (var deviceJson in await result.GetNextAsJsonAsync())
        //            {
        //                using (var stringReader = new StringReader(deviceJson))
        //                using (var jsonReader = new JsonTextReader(stringReader))
        //                {
        //                    var deviceInfo = jsonSerializer.Deserialize<DeviceItem>(jsonReader);
        //                    if (deviceInfo != null)
        //                        devices.Add(deviceInfo);
        //                }
        //            }
        //        }                                                    
        //        return devices;
        //    }
        //    catch (Exception ex) { Debug.WriteLine($"{ex.Message}"); }
        //    return null!;
        //}
        public async Task<IEnumerable<Twin>> GetDevicesAsTwinAsync(string sqlQuery = "select * from devices") // Metod för att hämta upp alla devices som en lista av Twins
        {
            try
            {
                var devices = new List<Twin>(); // hämta alla devices med en query. Använder en sql-lik query
                var result = _registryManager.CreateQuery(sqlQuery);  // får en iquery som resultat - en lista som vi vill loopa igenom
                if (result.HasMoreResults)
                    foreach (var device in await result.GetNextAsTwinAsync()) // hämta ut det som ett twinobjekt
                        devices.Add(device);                       // spara i en lista av twins 
                return devices;
            }
            catch (Exception ex) { Debug.WriteLine($"{ex.Message}"); }
            return null!;
        }
        //public async Task<Twin> GetDeviceAsTwinAsync(string deviceId) 
        //{
        //    try
        //    {
        //        var singleDevice = new Twin();
        //        var devices = new List<Twin>(); 
        //        var result = _registryManager.CreateQuery("select * from devices");  
        //        if (result.HasMoreResults)
        //            foreach (var device in await result.GetNextAsTwinAsync())
        //            {
        //                if(device.DeviceId == deviceId)
        //                    singleDevice = device;
        //            }                                          
        //        return singleDevice;
        //    }
        //    catch (Exception ex) { Debug.WriteLine($"{ex.Message}"); }
        //    return null!;
        //}
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
                        //catch (Exception ex){ Debug.WriteLine($"Fel: {ex.Message}");}
                        try
                        {
                            bool isActive = device.Properties.Reported["isActive"];
                            _device.IsActive = isActive;
                        }
                        catch (Exception ex) { Debug.WriteLine($"Fel: {ex.Message}"); }

                        Devices.Add(_device); // det kommer in devices med properties som det ska
                        updated = true;
                    }

                for (int i = Devices.Count - 1; i >= 0; i--)
                {
                    if (list.Any(x => x.Properties.Reported["isActive"] != Devices[i].IsActive))
                    {
                        Devices[i].IsActive = !Devices[i].IsActive;
                        updated = true;
                    }
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
        //public async Task<Device> GetDeviceAsync(string deviceId)
        //{
        //    try
        //    {
        //        var device = await _registryManager!.GetDeviceAsync(deviceId);
        //        if (device != null)
        //            return device;
        //    }
        //    catch (Exception ex) { Debug.WriteLine(ex.Message); }

        //    return null!;
        //}
        //public async Task<Device> RegisterDeviceAsync(string deviceId)
        //{
        //    try
        //    {
        //        var device = await _registryManager!.AddDeviceAsync(new Device(deviceId));
        //        if (device != null)
        //            return device;
        //    }
        //    catch (Exception ex) { Debug.WriteLine(ex.Message); }

        //    return null!;
        //}
        //public string GenerateConnectionString(Device device)
        //{
        //    try
        //    {   //tar första delen av hubens connstring - andra delen devicens id - sista delen devicens sharedaccesskey
        //        return $"{_connectionString.Split(";")[0]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";
        //    }
        //    catch (Exception ex) { Debug.WriteLine(ex.Message); }

        //    return null!;
        //}
    }
}
