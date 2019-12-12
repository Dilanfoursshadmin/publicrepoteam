using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class CompaniesType
    {
        [Key]
        public int CompanyId { get; set; }
        public string CompanyTypeName { get; set; }
        public string CompanyTypeStart { get; set; }
        public string CompanyTypeeMiddle { get; set; }
        public string CompanyTypeeEnd { get; set; }
    }
}