using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class CoolingOff
    {
        public int coolingOffId { get; set; }
        public string positionVal { get; set; }
        public int positionDetailId { get; set; }
        public int coolDownCount { get; set; }
        public DateTime coolDownDate { get; set; }
    }
}