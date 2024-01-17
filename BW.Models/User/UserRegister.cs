using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Models.User
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email {get; set;} = string.Empty;

        [Required, MinLength(5)]
        public string Password {get; set;} = string.Empty;
        [Compare(nameof(Password))]
        public string ConfirmPassword {get; set;} = string.Empty;

        [MaxLength(100)]
        public string FirstName {get; set;} = string.Empty;
        [MaxLength(100)]
        public string LastName {get; set;} = string.Empty;
        [Required]
        public string UserName {get; set;} = string.Empty;

    }
}