using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Data.Entities
{
    public class RatingEntity
    {
        [Key]
        public int Id {get; set;}

        [Required]
        [ForeignKey(nameof(BookEntity))]
        public int BookId {get; set;}

        [Required]
        [Range(1,5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int StarRating {get; set;}

        [MinLength(1), MaxLength(250)]
        public string Title {get; set;} = string.Empty;

        [MinLength(1), MaxLength(1000)]
        public string Comment {get; set;} = string.Empty;

        [ForeignKey(nameof(Owner))]
        public int OwnerId {get; set;}

        UserEntity Owner {get; set;} = null!;
    }
}