using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class IotHubManagerOptions
    {
        public string IotHubConnectionString { get; set; } = "HostName=MalinsIotDevice.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=YtL+BJzB3/jZ63a378FyZzHZw2B89lDzOAIoTFDCNP8=";   //null!;
        public string EventHubEndPoint { get; set; } = "Endpoint=sb://ihsuprodamres049dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=YtL+BJzB3/jZ63a378FyZzHZw2B89lDzOAIoTFDCNP8=;EntityPath=iothub-ehub-malinsiotd-25231991-006434fe96"; //null!;
        public string EventHubName { get; set; } = "iothub-ehub-malinsiotd-25231991-006434fe96"; // null!;
        public string ConsumerGroup { get; set; } = "serviceapplication"; // null!;
    }
}

