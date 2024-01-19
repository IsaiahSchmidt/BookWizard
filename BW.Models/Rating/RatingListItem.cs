using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Models.Rating
{
    public class RatingListItem
    {
        public int Id {get; set;}
        public int StarRating {get; set;}
        public string Title {get; set;} = string.Empty;
        public string Comment {get; set;} = string.Empty;
        public int BookId {get; set;}
    }
}