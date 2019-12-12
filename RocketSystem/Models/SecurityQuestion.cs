using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class SecurityQuestion
    {
        [Key]
        public int securityQuestionId { get; set; }
        public string securityQuestion1 { get; set; }
    }
}