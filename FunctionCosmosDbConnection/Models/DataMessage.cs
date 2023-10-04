using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionCosmosDbConnection.Models
{
    public class DataMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public double Temperature { get; set; }
        public double Humidity { get; set; }       
        public PartitionKey PartitionKey { get; set; } = 
            new PartitionKey("TemperatureMessage"); 
        // Guid.NewGuid().ToString());
    }
}
