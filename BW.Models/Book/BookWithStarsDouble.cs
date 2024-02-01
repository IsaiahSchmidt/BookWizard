using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Models.Book
{
    public class BookWithStarsDouble
    {
        public int Id {get; set;}
        public string Title {get; set;} = string.Empty;
        public string Author {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;
        public int Length {get; set;}
        public double StarRating {get; set;}
    }
}