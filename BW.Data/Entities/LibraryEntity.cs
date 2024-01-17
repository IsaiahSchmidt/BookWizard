using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Data.Entities
{
    public class LibraryEntity
    {
        [Key]
        public int Id {get; set;}

        [ForeignKey(nameof(Book))]
        public int BookId {get; set;}

        BookEntity Book {get; set;} = null!;

        [ForeignKey(nameof(User))]
        public int UserId {get; set;}

        UserEntity User {get; set;} = null!;
    }
}