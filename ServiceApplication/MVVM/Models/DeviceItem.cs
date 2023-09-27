

namespace ServiceApplication.MVVM.Models
{
    public class DeviceItem
    {

        public DeviceItem()
        {
            //SetIcon();
        }


        public string? DeviceId { get; set; } // med denna kan jag skicka directmeddelande
        public string? Placement { get; set; } //var devicen är placerad någonstans. 
        public string? DeviceType { get; set; } //var det är för typ av device det är, ex lampa, tempsensor etc. bestämma vilken typ av ikon man vill visa
        public bool? IsActive { get; set; } = false;
        public string? Icon => SetIcon();


        private string SetIcon()
        {
            // Förenklad version av metoden nedan
            return (DeviceType?.ToLower()) switch
            {
                "light" => "\uf0eb",
                "fan" => "\ue004",
                _ => "\uf2db",//unicode i C#
            };
        }
    }
}
