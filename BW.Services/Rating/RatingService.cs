using System.ComponentModel;
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
                OwnerId = _userId,
                BookId = rating.BookId
            };
            _dbContext.Ratings.Add(entity);
            var numberOfChanges = await _dbContext.SaveChangesAsync();
            if (numberOfChanges != 1)
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
                BookId = rating.BookId,
                OwnerId = _userId
            };
        }

        public async Task<IEnumerable<RatingListItem?>> GetRatingsByOwnerIdAsync(int ownerId)
        {
            List<RatingListItem> ratings = await _dbContext.Ratings
                .Where(entity => entity.OwnerId == ownerId)
                .Select(entity => new RatingListItem
                {
                    Id = entity.Id,
                    BookId = entity.BookId,
                    Title = entity.Title,
                    Comment = entity.Comment,
                    StarRating = entity.StarRating,
                    OwnerId = entity.OwnerId
                }).ToListAsync();
            return ratings;
        }

        public async Task<IEnumerable<RatingListItem?>> GetRatingsByBookIdAsync(int bookId)
        {
            List<RatingListItem> ratings = await _dbContext.Ratings
                .Where(entity => entity.BookId == bookId)
                .Select(entity => new RatingListItem
                {
                    Id = entity.Id,
                    OwnerId = entity.OwnerId,
                    Title = entity.Title,
                    Comment = entity.Comment,
                    StarRating = entity.StarRating,
                    BookId = entity.BookId
                }).ToListAsync();
            return ratings;
        }

        public async Task<bool> UpdateRatingAsync(RatingUpdate rating)
        {
            RatingEntity? entity = await _dbContext.Ratings.FindAsync(rating.Id);
            if (entity?.OwnerId != _userId)
                return false;
            entity.Title = rating.Title;
            entity.Comment = rating.Comment;
            entity.StarRating = rating.StarRating;

            int numberOfChanges = await _dbContext.SaveChangesAsync();
            return numberOfChanges == 1;
        }

        public async Task<bool> DeleteRatingAsync(int ratingId)
        {
            var ratingEntity = await _dbContext.Ratings.FindAsync(ratingId);
            if (ratingEntity?.OwnerId != _userId)
                return false;
            _dbContext.Ratings.Remove(ratingEntity);
            return await _dbContext.SaveChangesAsync() == 1;
        }
    }
}