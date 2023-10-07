using ServiceApplication.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApplication.MVVM.ViewModels
{
    public class DeviceItemViewModel
    {
        private DeviceItem _deviceItem;

        public DeviceItemViewModel(DeviceItem deviceItem)
        {
            _deviceItem = deviceItem;
        }
    }
}
