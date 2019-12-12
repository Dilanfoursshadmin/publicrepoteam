using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class PaidBonus
    {
        public int paidBonusId { get; set; }
        public string memberId { get; set; }
        public int bcNumber { get; set; }
        public DateTime paidBonusDateTime { get; set; }
        public double paidIntoduceBonus { get; set; }
        public double paidPresentageBonus { get; set; }
        public double paidThirdStageBonus { get; set; }
        public double paidFifthStageBonus { get; set; }
        public double paidpositionBonus { get; set; }
        public double paidshareBonus { get; set; }
        public int positionHistory { get; set; }
        public int complteorno { get; set; }
        public int stageonetreeid { get; set; }
    }
}