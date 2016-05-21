using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Delivery.Models
{
    public class Rewards
    {
        [Key]
        public int ID { get; set; }
        //[ForeignKey("Order")]
        //public int OrderID { get; set; }

        [Display(Name ="酬金类型")]
        [StringLength(50)]
        public string Type  { get; set; }
        public int Money { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }

     // public virtual Orders Order { get; set; }
    }
}