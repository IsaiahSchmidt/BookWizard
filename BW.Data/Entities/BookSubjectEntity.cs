using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Data.Entities
{
    public class BookSubjectEntity
    {
        [Key]
        public int Id {get; set;}

        [Required]
        [ForeignKey(nameof(Book))]
        public int BookId {get; set;}

        BookEntity Book {get; set;} = null!;

        [Required]
        [ForeignKey(nameof(Subject))]
        public int SubjectId {get; set;}

        SubjectEntity Subject {get; set;} = null!;
    }
}