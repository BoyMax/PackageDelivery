using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Delivery.Models
{
    public class Packages
    {
        [Key]
        public int ID { get; set; }      
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        [StringLength(50)]
        public string ExpressCompany { get; set; }
        [StringLength(200)]
        public string Description { get; set; }

        public virtual Orders Order { get; set; }
    }
}