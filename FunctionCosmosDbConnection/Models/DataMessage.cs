using Microsoft.Azure.Cosmos;

namespace FunctionCosmosDbConnection.Models
{
    public class DataMessage
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string? DeviceId { get; set; }
        public double Temperature { get; set; }
        public string? DeviceType { get; set; }
        public bool IsActive { get; set; }
        public int TelemetryInterval { get; set; }
        public DateTime Created { get; set; }
        public int _ts { get; set; }
    }
}
