using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginTestDemo.Models
{
    public class ResetPasswordModel
    {
        [Key]
        [Required(ErrorMessage = "Enter New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string OTP { get; set; }
    }
}