using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Models.Book
{
    public class BookCreate
    {
        [Required]
        [MaxLength(600, ErrorMessage = "Name must be no more than 600 characters")] 
        [MinLength(1, ErrorMessage = " Must be at least 1 character")]
        public string Name {get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Must be no more than 250 characters")] 
        [MinLength(1, ErrorMessage = " Must be at least 1 character")]
        public string Author {get; set;} = string.Empty;

        [MaxLength(1000, ErrorMessage = "Description must be no more than 1,000 characters")]
        public string Description {get; set;} = string.Empty;

        public int Length {get; set;}
    }
}