using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class ExcelDataModel
    {
        [Required]
        public HttpPostedFileBase MyExcelFile { get; set; }

        public string MSExcelTable { get; set; }
    }
}