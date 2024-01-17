using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Data.Entities
{
    public class SubjectEntity
    {
        [Key]
        public int Id {get; set;}

        [Required]
        [MinLength(1), MaxLength(100)]
        public string Name {get; set;} = string.Empty;
    }
}