
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApplication.MVVM.ViewModels
{
    public class DeviceItemViewModel
    {
        public DeviceItem DeviceItem { get; private set; } = new DeviceItem();

        public DeviceItemViewModel(DeviceItem deviceItem)
        {
            DeviceItem = deviceItem;
        }
    }
}
