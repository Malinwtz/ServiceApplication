using Microsoft.Azure.Cosmos;

namespace FunctionCosmosDbConnection.Models
{
    public class DataMessage
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public double Temperature { get; set; } // denna skickas inte med fårn devicen - ta bort?
        public double Humidity { get; set; }// denna skickas inte med fårn devicen
        public int _ts { get; set; }
    }
}
