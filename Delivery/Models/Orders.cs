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
        [Key]
        public int ID { get; set; }
        [ForeignKey("Sender")]
        public int SenderID { get; set; }
        [ForeignKey("Receiver")]
        public int ReceiverID { get; set; }
        [ForeignKey("PickLocation")]
        [Required]
        public int PickLocationID { get; set; }
        [Required]
        [ForeignKey("ReceiverLocation")]
        public int ReceiverLocationID { get; set; }
        [ForeignKey("Reward")]
        public int RewardID { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        public int Mark { get; set; }
        [StringLength(200)]
        public string Comment { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime PublishTime { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime EndTime { get; set; }

        public virtual Users Sender { get; set; }
        public virtual Users Receiver { get; set; }
        public virtual Locations PickLocation { get; set; }
        public virtual Locations ReceiverLocation { get; set; }
        public virtual Rewards Reward { get; set; }
    }
}