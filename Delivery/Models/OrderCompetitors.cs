using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Delivery.Models
{
    public class OrderCompetitors
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }
        [ForeignKey("Order")]
        [Required]
        public int OrderID { get; set; }
        [ForeignKey("Location")]
        [Required]
        public int LocationID { get; set; }

        public virtual Users User { get; set; }
        public virtual Orders Order { get; set; }
        public virtual Locations Location { get; set; }

    }
}