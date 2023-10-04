using System;
using System.Text;
using Azure.Messaging.EventHubs;
using FunctionCosmosDbConnection.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionCosmosDbConnection.Methods
{
    public class SaveDataToCosmosDb
    {
        private readonly ILogger<SaveDataToCosmosDb> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public SaveDataToCosmosDb(ILogger<SaveDataToCosmosDb> logger)
        {
            _logger = logger;

            _cosmosClient = new CosmosClient("AccountEndpoint=https://malins-cosmosdb.documents.azure.com:443/;AccountKey=jUj74N15medMT2LhBOkqpMPGsGJcFMp5M97k69zTeylT2GqCyl9NNuSr5XikuIASKqSicp8cfVvAACDbA8JCjg==;");
            var database = _cosmosClient.GetDatabase("deviceDb");
            _container = database.GetContainer("devicemessages");
        }

        [Function(nameof(SaveDataToCosmosDb))]
        public async Task Run([EventHubTrigger(
            "iothub-ehub-malinsiotd-25231991-006434fe96", 
            Connection = "IotHubEndPoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                try
                {
                    var json = Encoding.UTF8.GetString(@event.Body.ToArray());  //gör om en bytearray till en läsbar sträng
                    var data = JsonConvert.DeserializeObject<DataMessage>(json);

                    //data.id = Guid.NewGuid().ToString(); // Set a unique ID 
                    await _container.CreateItemAsync(data, new PartitionKey(data.id));

                    _logger.LogInformation($"Sparade meddelandet: {data}"); //skriver ut json-kod
                }
                catch ( Exception ex )
                {
                    _logger.LogInformation($"Kunde inte spara. Felmeddelande: {ex.Message}");
                }
            }        
        }
    }
}


// Connstringarna ska ligga i localsettingsfilen. Se tidigare projekt.

