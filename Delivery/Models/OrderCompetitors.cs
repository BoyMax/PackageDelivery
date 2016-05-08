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
        [ForeignKey("User")]
        [Required]
        public Users UserID { get; set; }
        [Key]
        [ForeignKey("Order")]
        [Required]
        public Orders OrderID { get; set; }

        public virtual Users User { get; set; }
        public virtual Orders Order { get; set; }
    }
}