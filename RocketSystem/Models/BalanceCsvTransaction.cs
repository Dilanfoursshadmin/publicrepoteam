using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class BalanceCsvTransaction
    {
        public int BalanceCsvTransactionId { get; set; }
        public int memberBalanceTransactionId { get; set; }
        public int csvDataId { get; set; }
        public int positionId { get; set; }
        public int secondCsvDataId { get; set; }
    }
}