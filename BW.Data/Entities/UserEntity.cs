using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BW.Data.Entities
{
    public class UserEntity : IdentityUser<int>
    {
    [Key]
    public int Id {get; set;}
    
    [Required]
    public string Email {get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName {get; set;} = string.Empty;
    [MaxLength(100)]
    public string? LastName {get; set;} = string.Empty;
    public DateTime DateCreated {get; set;}
    }
}