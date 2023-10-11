using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SharedLibrary.Services;

namespace ConnectDeviceToHubFunction.Actions
{
    public class DeviceRegistration
    {
        private readonly IotHubManager _iotHubManager; // ska få tillgång till modeller, services och context-delar

        public DeviceRegistration(IotHubManager iotHubManager)
        {
            _iotHubManager = iotHubManager;
            _iotHubManager.Initialize(
                "HostName=MalinsIotDevice.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=YtL+BJzB3/jZ63a378FyZzHZw2B89lDzOAIoTFDCNP8=");
        }


        [Function("DeviceRegistration")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {   //deviceid kommer skickas med via post-anrop i urlen
            var deviceId = req.Query["deviceId"]; 
            if (!string.IsNullOrEmpty(deviceId))
            {
                var device = await _iotHubManager.GetDeviceAsync(deviceId); // hämtar devicen med detta id
                device ??= await _iotHubManager.RegisterDeviceAsync(deviceId); // gör en ny device med detta id - returnera device
                                
                if (device != null)
                {   // om vi har en device vill vi bygga upp en connstring
                    var connectionString = _iotHubManager.GenerateConnectionString(device);

                    if (!string.IsNullOrEmpty(connectionString))
                        return GenerateHttpResponse(req, HttpStatusCode.OK, connectionString);
                    else
                        return GenerateHttpResponse(req, HttpStatusCode.BadRequest, "An error occured! Connectionstring was not created.");
                }
            }
            return GenerateHttpResponse(req, HttpStatusCode.BadRequest, "An error occured! Parameter deviceId is required.");
        }


        private HttpResponseData GenerateHttpResponse(HttpRequestData req, HttpStatusCode statusCode, string content)
        {
            var response = req.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(content);
            return response;
        }
    }
}



// Funktionen ska kontrollera om det finns en device eller inte genom httptrigger.

// Genom att använda Function som authorization level så får vi en nyckel i slutet av vår "function-del". Denna nyckel kan sedan sparas ner i någon lagringslösning. 