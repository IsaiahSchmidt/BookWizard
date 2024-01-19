
using BW.Models.Rating;

namespace BW.Services.Rating
{
    public interface IRatingService
    {
        Task<RatingListItem?> CreateRatingAsync(RatingCreate rating);
        Task<RatingDetail?> GetRatingByIdAsync(int ratingId);
        Task<bool> DeleteRatingAsync(int ratingId);
    }
}