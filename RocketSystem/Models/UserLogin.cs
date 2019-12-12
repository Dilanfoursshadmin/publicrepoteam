using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RocketSystem.Models
{
    public class UserLogin
    {
        [Key]
        public int userLogID { get; set; }
        [Required(ErrorMessage = "会員番号が必要です。")]
        public string membershipNo { get; set; }
        public string userName { get; set; }
        [Required(ErrorMessage = "パスワードが必要です。")]
        public string password { get; set; }
        public int questionOne { get; set; }
        public string answerOne { get; set; }
        public int questionTwo { get; set; }
        public string answerTwo { get; set; }
        public int status { get; set; }
        public DateTime datetime { get; set; }
        public int uID { get; set; }
    }

    public class ForgetQuestion
    {
        [Key]
        public int sqID { get; set; }
        [Required(ErrorMessage = "第一答えが必要です。")]
        [Display(Name = "回答 1")]
        public string sq1 { get; set; }
        [Required(ErrorMessage = "T答えが二つ必要です。")]
        [Display(Name = "回答 2")]
        public string sq2 { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Key]
        public int chgPwID { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Enter Old password")]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "パスワードが必要です。")]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "パスワードの確認が必要です。")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "パスワードには文字が8個必要です", MinimumLength = 8)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "パスワードが一致しないのでタイプしてください。")]
        public string password1 { get; set; }

    }

    public class LoginAsFourssh
    {
        [Key]
        public int loginAsFoursshId { get; set; }
        [Required(ErrorMessage = "パスワードが必要です。")]
        public string membershipNo { get; set; }
        [Required(ErrorMessage = "パスワードが必要です。")]
        public string password { get; set; }
    }

    public class TempUseLogin
    {
        [Key]
        public int userLogID { get; set; }
        public string membershipNo { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
        public int questionOne { get; set; }
        public string answerOne { get; set; }
        public int questionTwo { get; set; }
        public string answerTwo { get; set; }
        public int status { get; set; }
        public DateTime datetime { get; set; }
        public int uID { get; set; }
    }
}