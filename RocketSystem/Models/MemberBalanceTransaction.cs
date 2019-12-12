using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class MemberBalanceTransaction
    {
        public int memberBalanceTransactionId { get; set; }
        public string memberId { get; set; }
        public int balanceAmount { get; set; }
        public DateTime transactionDate { get; set; }
        public string creditOrDebit { get; set; }
    }
}