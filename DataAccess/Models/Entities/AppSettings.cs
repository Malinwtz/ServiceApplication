using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Entities
{
    public class AppSettings
    {
        [Key]
        public string ConnectionString { get; set; } = null!; // till vår iot hub
    }
}
