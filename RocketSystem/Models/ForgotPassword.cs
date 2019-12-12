using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class ForgotPassword
    {
        [Key]
        public int ForgotPasswordID { get; set; }
        public string memNo { get; set; }
        public string ip { get; set; }
        public System.DateTime date { get; set; }
    }
}