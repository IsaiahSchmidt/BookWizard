using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Models.Rating
{
    public class RatingUpdate
    {
        [Required]
        public int Id {get; set;}

        [Required]
        [Range(1,5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int StarRating {get; set;}

        [MinLength(1), MaxLength(250)]
        public string Title {get; set;} = string.Empty;

        [MinLength(1), MaxLength(1000)]
        public string Comment {get; set;} = string.Empty;
    }
}