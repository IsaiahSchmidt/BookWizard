using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Models.Book
{
    public class BookListItem
    {
         public int Id {get; set;}
        public string Name {get; set; } = string.Empty;
        public string Author {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;
        public int Length {get; set;}
    }
}