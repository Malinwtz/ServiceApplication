using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionCosmosDbConnection
{
    public class SaveDataToCosmosDb
    {
        private readonly ILogger<SaveDataToCosmosDb> _logger;

        public SaveDataToCosmosDb(ILogger<SaveDataToCosmosDb> logger)
        {
            _logger = logger;
        }

        [Function(nameof(SaveDataToCosmosDb))]
        public void Run([EventHubTrigger("iothub-ehub-malinsiotd-25231991-006434fe96", Connection = "IotHubEndPoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                _logger.LogInformation("Event Body: {body}", @event.Body);
                _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
            }
        }
    }
}
