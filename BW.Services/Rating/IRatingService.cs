
using BW.Models.Rating;

namespace BW.Services.Rating
{
    public interface IRatingService
    {
        Task<RatingListItem?> CreateRatingAsync(RatingCreate rating);
    }
}