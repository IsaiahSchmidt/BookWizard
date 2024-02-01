
namespace BW.Models.Rating
{
    public class RatingDetail
    {
        public int Id {get; set;}
        public int StarRating {get; set;}
        public string Title {get; set;} = string.Empty;
        public string Comment {get; set;} = string.Empty;
        public int BookId {get; set;}
        public int OwnerId {get; set;}
    }
}