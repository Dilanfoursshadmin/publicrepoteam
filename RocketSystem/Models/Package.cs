using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class Package
    {
        public int packageId { get; set; }
        public string packageName { get; set; }
        public int noOfPosition { get; set; }
        public DateTime packageStartDate { get; set; }
        public DateTime packageEndDate { get; set; }
        public int stage { get; set; }
        public int status { get; set; }
        public int packagePriority { get; set; }
        public int packagePrize { get; set; }
    }
}