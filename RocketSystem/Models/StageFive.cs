using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class StageFive
    {
        [Key]
        public int treeId { get; set; }
        public string membershipNo { get; set; }
        public int bcNo { get; set; }
        public int treeLevel { get; set; }
        public Int64 treeColumn { get; set; }
        public int jump { get; set; }
        public DateTime entryDate { get; set; }
        public int previousPosition { get; set; }
        public int package { get; set; }
        public int bonus5;
        public string jumpHistory { get; set; }
    }
}