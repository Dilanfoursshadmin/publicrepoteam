using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class Bonus
    {
        public int bonusId { get; set; }
        public string membershipNo { get; set; }
        public string bonusStatus { get; set; }
        public string totalBonus { get; set; }
    }

    public class ApiMyCoin
    {
        public double coin { get; set; }
    }
}