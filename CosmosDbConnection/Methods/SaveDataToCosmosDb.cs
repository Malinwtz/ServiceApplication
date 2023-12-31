using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CosmosDbConnection.Methods
{
    public class SaveDataToCosmosDb
    {
        private readonly ILogger<SaveDataToCosmosDb> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public SaveDataToCosmosDb(ILogger<SaveDataToCosmosDb> logger)
        {
            _logger = logger;

            //l�gg till 
            _cosmosClient = new CosmosClient("AccountEndpoint=https://malins-cosmosdb.documents.azure.com:443/;AccountKey=jUj74N15medMT2LhBOkqpMPGsGJcFMp5M97k69zTeylT2GqCyl9NNuSr5XikuIASKqSicp8cfVvAACDbA8JCjg==;");
            var database = _cosmosClient.GetDatabase("deviceDb");
            _container = database.GetContainer("devicemessages");
        }

        [Function(nameof(SaveDataToCosmosDb))]
        public async Task Run([EventHubTrigger("iothub-ehub-malinsiotd-25231991-006434fe96", 
            Connection = "IotHub")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                try
                {
                    var json = Encoding.UTF8.GetString(@event.Body.ToArray());  //g�r om en bytearray till en l�sbar str�ng
                    var data = JsonConvert.DeserializeObject<DataMessage>(json);
                    await _container.CreateItemAsync(data, new PartitionKey(data.Id)); // vill ha in en str�ng. 

                    _logger.LogInformation($"Sparade meddelandet: {data}"); //skriver ut json-kod
                }
                catch
                {
                    _logger.LogInformation("Kunde inte spara");
                }
            }

            //foreach (EventData @event in events)
            //{
            //    _logger.LogInformation("Event Body: {body}", @event.Body);
            //    _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
            //}
        }
    }
}
