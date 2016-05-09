using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Models
{
    public class OrderCreateViewModel
    {
        [Required]
        [Display(Name = "经度")]
        public float Longitude { get; set; }
        [Required]
        [Display(Name = "纬度")]
        public float Latitude { get; set; }
        [StringLength(100)]
        [Display(Name = "地点")]
        public string PlaceName { get; set; }
        [StringLength(200)]
        [Display(Name = "地点备注")]
        public string Remark { get; set; }

        [StringLength(50)]
        [Display(Name = "快递公司")]
        public string ExpressCompany { get; set; }
        [StringLength(200)]
        [Display(Name = "包裹备注")]
        public string Description { get; set; }
    }
}
   