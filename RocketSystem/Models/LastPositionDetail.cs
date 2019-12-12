using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class LastPositionDetail
    {
        public int LastPositionDetailId { get; set; }
        public int positionId { get; set; }
        public DateTime positionDate { get; set; }
    }
}