using BW.Data;
using BW.Data.Entities;
using BW.Models.Rating;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BW.Services.Rating
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly int _userId;

        public RatingService(UserManager<UserEntity> userManager,
                             SignInManager<UserEntity> signInManager,
                             ApplicationDbContext dbContext)
        {
            var currentUser = signInManager.Context.User;
            var userIdClaim = userManager.GetUserId(currentUser);
            var hasValidId = int.TryParse(userIdClaim, out _userId);
            if (hasValidId == false)
                throw new Exception("Attempted to build RatingService without ID claim");

            _dbContext = dbContext;
        } 

        public async Task<RatingListItem?> CreateRatingAsync(RatingCreate rating)
        {
            RatingEntity entity = new()
            {
                Title = rating.Title,
                Comment = rating.Comment,
                StarRating = rating.StarRating,
                OwnerId = _userId
            };
            _dbContext.Ratings.Add(entity);
            var numberOfChanges = await _dbContext.SaveChangesAsync();
            if(numberOfChanges != 1)
                return null;
            
            RatingListItem ratingListItem = new()
            {
                Id = entity.Id,
                Title = entity.Title,
                Comment = entity.Comment,
                StarRating = entity.StarRating,
                BookId = entity.BookId
            };
            return ratingListItem;
        }

        public async Task<RatingDetail?> GetRatingByIdAsync(int ratingId)
        {
            RatingEntity? rating = await _dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == ratingId
                && r.OwnerId == _userId);

            return rating is null ? null : new RatingDetail
            {
                Id = rating.Id,
                Title = rating.Title,
                Comment = rating.Comment,
                StarRating = rating.StarRating,
                BookId = rating.BookId
            };
        }
    }
}