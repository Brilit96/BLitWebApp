using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BLitWebApp.Models
{
    public class CarClass
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
