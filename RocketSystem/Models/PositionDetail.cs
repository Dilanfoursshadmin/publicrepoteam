using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class PositionDetail
    {
        [Key]
        public int positionId { get; set; }
        public string membershipNo { get; set; }
        public string introducePromoCode { get; set; }
        public DateTime registerDate { get; set; }
        public DateTime depositDate { get; set; }
        public string package { get; set; }
        public int packageStyle { get; set; }
        public int positionCount { get; set; }
        public int paymentStatus { get; set; }
        public int systemUpdate { get; set; }
        public string positionStatus { get; set; }
        public int positionPriority { get; set; }
    }
}