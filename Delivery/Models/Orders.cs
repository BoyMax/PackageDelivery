using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Delivery.Models
{
    public class Orders
    {

        public Orders()
        {
            this.PublishTime = DateTime.Now;
        }
        [Key]
        public int ID { get; set; }

       
        [ForeignKey("Sender")]
        public int? SenderID { get; set; }

        [ForeignKey("Receiver")]
        public int? ReceiverID { get; set; }

        [Display(Name = "领取地点")]
        [ForeignKey("PickLocation")]
        [Required]
        public int? PickLocationID { get; set; }

        [Display(Name = "代收地点")]
        [ForeignKey("ReceiverLocation")]
        public int? ReceiverLocationID { get; set; }

        [Display(Name = "酬金")]
        [ForeignKey("Reward")]
        public int? RewardID { get; set; }

        [Display(Name = "状态")]
        [StringLength(20)]
        public string Status { get; set; }

        [Display(Name = "评分")]
        public int Mark { get; set; }

        [Display(Name = "评论")]
        [StringLength(200)]
        public string Comment { get; set; }

        [Display(Name = "公告时间")]
        [Column(TypeName = "datetime2")]
        public DateTime PublishTime { get; set; }

        [Display(Name = "接单时间")]
        [Column(TypeName = "datetime2")]
        public DateTime EndTime { get; set; }

        public virtual ICollection<Packages> Packages { get; set; }
        public virtual ICollection<OrderCompetitors> OrderCompetitors { get; set; }

 
        public virtual Users Sender { get; set; }
        public virtual Users Receiver { get; set; }
        public virtual Locations PickLocation { get; set; }
        public virtual Locations ReceiverLocation { get; set; }
        public virtual Rewards Reward { get; set; }
    }
}