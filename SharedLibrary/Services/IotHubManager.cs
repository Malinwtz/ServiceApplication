using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    // Här hav vi gjort en serviceclient som kan ta emot ett directMethod-meddelande
    // och skicka iväg det till en specifik adress. Vi behöver även en applikation
    // där vi kan köra det här ifrån - WPF Applikation (maui fungerar också) WPFService
    // hanterar all funktionalitet som har med iot huben att göra.
    // Ex registrera enheter, skicka directmethod-meddelanden

    // >>>>>>    D13 1:55 anropa sqlite databas för att hitta connectionstring
    //              3:25 azure function för att automatiskt koppla devices till huben via databasens connectionstring <<<<<<<
    public class IotHubManager
    {
        private RegistryManager _registryManager; //från nuget Devices
        private ServiceClient _serviceClient; //från nuget Devices
        private EventHubConsumerClient _consumerClient; //från nuget

        public IotHubManager(IotHubManagerOptions options)
        {
            _registryManager = RegistryManager.CreateFromConnectionString(options.IotHubConnectionString);
            _serviceClient = ServiceClient.CreateFromConnectionString(options.IotHubConnectionString);
            _consumerClient = new EventHubConsumerClient(options.ConsumerGroup, options.EventHubEndPoint);
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

    }
}
