
using BW.Models.Rating;

namespace BW.Services.Rating
{
    public interface IRatingService
    {
        Task<RatingListItem?> CreateRatingAsync(RatingCreate rating);
        Task<RatingDetail?> GetRatingByIdAsync(int ratingId);
        Task<IEnumerable<RatingListItem?>> GetRatingsByOwnerIdAsync(int ownerId);
        Task<IEnumerable<RatingListItem?>> GetRatingsByBookIdAsync(int bookId);
        Task<bool> UpdateRatingAsync(RatingUpdate rating);
        Task<bool> DeleteRatingAsync(int ratingId);
    }
}