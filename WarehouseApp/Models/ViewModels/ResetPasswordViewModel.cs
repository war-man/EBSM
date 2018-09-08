using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        public int UserId { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^.*(?=^.{6,64}$)((?=.*[a-z])|(?=.*[A-Z]))((?=.*\d)(?=.*[\W])).*$", ErrorMessage = "Password must be minimum 6 character long and must contain atleast 1 number and special character")]

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePsswordViewModel
    {
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^.*(?=^.{8,12}$)((?=.*[a-z])|(?=.*[A-Z]))((?=.*\d)|(?=.*[\W])).*$", ErrorMessage = "Password must be 8-12 character long and must contain atleast 1 number or special character")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
   
}


//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : December 2016
//=======================================================================================//