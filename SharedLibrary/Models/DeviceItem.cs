using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class DeviceItem
    {
        public DeviceItem()
        {
        }

        public string DeviceId { get; set; } = null!;
        public string DeviceType { get; set; } = null!; 
        public string? Placement { get; set; } 
        public bool? IsActive { get; set; } = false;
        public string? Icon => SetIcon();

        private string SetIcon()
        {
            return (DeviceType?.ToLower()) switch
            {
                "light" => "\uf0eb",
                "fan" => "\ue004",
                _ => "\uf2db",
            };
        }
    }
}
