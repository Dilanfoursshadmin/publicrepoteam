using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class UserSuspendHistory
    {
        [Key]
        public int userSuspendId { get; set; }
        public int userId { get; set; }
        public int adminId { get; set; }
        public DateTime dateHistory { get; set; }
        public int status { get; set; }
    }
}