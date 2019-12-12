using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class BonusDetail
    {
        public int bonusDetailId { get; set; }
        public int bonusId { get; set; }
        public string description { get; set; }
        public int bonusAmount { get; set; }
        public DateTime changeBonusDate { get; set; }
    }
}