using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Delivery.Models
{
    public class Locations
    {
        [Key]
        public int ID { get; set; }
        [StringLength(100)]
        public string PlaceName { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
    }
}