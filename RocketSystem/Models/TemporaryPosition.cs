using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class TemporaryPosition
    {
        public int temporaryPositionId { get; set; }
        public int positionId { get; set; }
        public string positionCode { get; set; }
    }
}