using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class User
    {
        public int userId { get; set; }
        public string name { get; set; }
        public string membershipNo { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string telephoneNo { get; set; }
        public string mobileNo { get; set; }
        public string postalCode { get; set; }
        public string katakanaName { get; set; }
        public string nickName { get; set; }
        public string gender { get; set; }
        public string addressOne { get; set; }
        public string addressTwo { get; set; }
        public string addressThree { get; set; }
        public string faxNo { get; set; }
        public string webEmail { get; set; }
        public string mobileEmail { get; set; }
        public string accountName { get; set; }
        public string accountNameKatakana { get; set; }
        public string bankNameKatakana { get; set; }
        public string transferDestinationBank { get; set; }
        public string branchNameKatakana { get; set; }
        public string transferDestinationBranchCode { get; set; }
        public string accountClassification { get; set; }
        public string accountClassification2 { get; set; }
        public string transferAccountNumber { get; set; }
        public string userName { get; set; }
        public Nullable<int> companyType { get; set; }
        public string foursshMember { get; set; }
    }

    public class FogotMemNo
    {
        public int fogotId { get; set; }
        [Required(ErrorMessage = "date of birth required")]
        public DateTime dateOfBirth { get; set; }
        [Required(ErrorMessage = "katakanaName required")]
        public string katakanaName { get; set; }

    }

}