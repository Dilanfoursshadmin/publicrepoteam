using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class CsvData
    {
        public int csvDataId { get; set; }
        public DateTime csvDate { get; set; }
        public string transactionDetail { get; set; }
        public int acceptedamount { get; set; }
        public string outAmount { get; set; }
        public string detailOne { get; set; }
        public string detailTwo { get; set; }
        public string accountBalance { get; set; }
        public DateTime csvStartDate { get; set; }
        public DateTime csvEndDate { get; set; }
        public string status { get; set;  }
    }
}