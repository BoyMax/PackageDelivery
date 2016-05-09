using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Delivery.Models
{
    public class Addresses
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }
        [ForeignKey("Address")]
        [Required]
        public int AddressesID { get; set; }

        public virtual Users User { get; set; }
        public virtual Locations Address { get; set; }
    }
}