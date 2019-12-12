using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class TreeTableModel
    {
        public int treeTableModelId { get; set; }
        public string membershipNo { get; set; }
        public int bcNo { get; set; }
        public int treeLevel { get; set; }
        public int treeColumn { get; set; }
        public int jump { get; set; }
        public string positionCode { get; set; }
        public string introducePromoCode { get; set; }
        public DateTime entryDate { get; set; }
        public int package { get; set; }
        public int freeStatus { get; set; }
        public int bonus;
    }

    public partial class StageTwoView
    {
        public int treeId { get; set; }
        public string membershipNo { get; set; }
        public int bcNo { get; set; }
        public int treeLevel { get; set; }
        public int treeColumn { get; set; }
        public int jump { get; set; }
        public DateTime entryDate { get; set; }
        public int previousPosition { get; set; }
        public int package { get; set; }
    }
}