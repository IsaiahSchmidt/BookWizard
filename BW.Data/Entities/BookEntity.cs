using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Data.Entities
{
    public class BookEntity
    {
        [Key]
        public int Id {get; set;}

        [Required]
        [MaxLength(600), MinLength(1)]
        public string Title {get; set; } = string.Empty;

        [MaxLength(250), MinLength(1)]
        public string Author {get; set;} = string.Empty;

        [MaxLength(2000)]
        public string Description {get; set;} = string.Empty;

        public int Length {get; set;}
    }
}