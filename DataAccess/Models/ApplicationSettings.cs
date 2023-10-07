using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class ApplicationSettings
    {
        [Key] 
        public string ConnectionString { get; set; } = null!; // till vår iot hub
    }
}
