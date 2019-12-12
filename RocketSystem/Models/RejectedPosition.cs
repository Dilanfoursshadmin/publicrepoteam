using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class RejectedPosition
    {
        public int rejectedPositionId { get; set; }
        public int positionId { get; set; }
        public DateTime rejectedDateTime { get; set; }
        public string adminId { get; set; }
        public string status { get; set; }
    }
}