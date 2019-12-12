using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class Kalamanakaru
    {
        [Key]
        public int adminId { get; set; }
        public string adminName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string telephoneNo { get; set; }
        public string address { get; set; }
        public string adminEmail { get; set; }
        public int adminLevel { get; set; }
        [Display(Name = "User Name")]
        public string userName { get; set; }
        [Display(Name = "Password")]
        public string password { get; set; }
    }
}