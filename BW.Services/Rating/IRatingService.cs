
using BW.Models.Rating;

namespace BW.Services.Rating
{
    public interface IRatingService
    {
        Task<RatingListItem?> CreateRatingAsync(RatingCreate rating);
        Task<RatingDetail?> GetRatingByIdAsync(int ratingId);
        Task<IEnumerable<RatingListItem?>> GetRatingsByOwnerIdAsync(int ownerId);
        // Task<List<RatingDetail?>> GetRatingByBookIdAsync(int bookId); //! -----Fix------
        Task<bool> UpdateRatingAsync(RatingUpdate rating);
        Task<bool> DeleteRatingAsync(int ratingId);
    }
}