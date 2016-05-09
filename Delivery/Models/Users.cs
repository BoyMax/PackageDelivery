using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Delivery.Models
{
    public class Users
    {
        [Key]
        public int ID { get; set; }
        [StringLength(20)]
        public string Account { get; set; }
        [StringLength(20)]
        public string Password { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Sex { get; set; }
        [StringLength(20)]
        public string Grade { get; set; }
        [StringLength(20)]
        public string Degree { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Locations> Locations { get; set; }
    }
}