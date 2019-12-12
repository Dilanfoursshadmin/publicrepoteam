using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class MemberDetail
    {
        public int memberDetailId { get; set; }
        public string MemberNo { get; set; }
        public string bcNo { get; set; }
        public string IntroducerMemberNo { get; set; }
        public string IntroducerBcNo { get; set; }
        public string parentNo { get; set; }
        public string parentBcNo { get; set; }
        public string column1 { get; set; }
        public string registeredDate { get; set; }
        public string course { get; set; }
        public string name { get; set; }
        public string nameInKana { get; set; }
        public string nickName { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public string postalNo { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string telephoneNo { get; set; }
        public string mobileNo { get; set; }
        public string faxNo { get; set; }
        public string email { get; set; }
        public string nameOfTheAccountHolder { get; set; }
        public string nameOfTheAccountHolderInKana { get; set; }
        public string bankNameKana { get; set; }
        public string transferDestinationBankCode { get; set; }
        public string branchNameKana { get; set; }
        public string transferDestinationBranchCode { get; set; }
        public string accountClassification { get; set; }
        public string transferAccountNumber { get; set; }
        public string smsStatus { get; set; }
        public string recipientNameInKatakana { get; set; }
        public string accountClassification2 { get; set; }
        public int companyType { get; set; }
        public string password { get; set; }
    }
}