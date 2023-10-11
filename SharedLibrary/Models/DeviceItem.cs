﻿using System;
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
            //SetIcon();
        }


        public string DeviceId { get; set; } = null!; // med denna kan jag skicka directmeddelande
        public string DeviceType { get; set; } = null!; //var det är för typ av device det är, ex lampa, tempsensor etc. bestämma vilken typ av ikon man vill visa
        public string? Placement { get; set; } //var devicen är placerad någonstans. 
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