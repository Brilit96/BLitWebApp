using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BLitWebApp.Models
{
    public class Vehicle
    {
        public int ID { get; set; }

        [Required]
        [StringLength(17, MinimumLength =17)]
        public string VIN { get; set; }
        public int CarModelID { get; set; }
        public int CarClassID { get; set; }

        public CarModel CarModel { get; set; }
        public CarClass CarClass { get; set; }
    }
}
