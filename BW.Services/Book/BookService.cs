using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BW.Data;
using BW.Data.Entities;
using BW.Models.Book;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;

namespace BW.Services.Book
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly int _userId;

        public BookService(UserManager<UserEntity> userManager,
                            SignInManager<UserEntity> signInManager,
                            ApplicationDbContext dbContext)
        {
            var currentUser = signInManager.Context.User;
            var userIdClaim = userManager.GetUserId(currentUser);
            var hasValidId = int.TryParse(userIdClaim, out _userId);
            if (hasValidId == false)
            {
                throw new Exception("Attempted to build BookService without Id Claim");
            }

            _dbContext = dbContext;
        }

        public async Task<BookListItem?> CreateBookAsync(BookCreate book)
        {
            BookEntity entity = new()
            {
                Name = book.Name,
                Author = book.Author,
                Description = book.Description,
                Length = book.Length
            };
            _dbContext.Books.Add(entity);
            var numberOfChanges = await _dbContext.SaveChangesAsync();

            if (numberOfChanges != 1)
            {
                return null;
            }

            BookListItem response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Author = entity.Author,
                Description = entity.Description,
                Length = entity.Length
            };
            return response;
        }
    }
}