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
using Microsoft.EntityFrameworkCore;

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
                Title = book.Title,
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
                Title = entity.Title,
                Author = entity.Author,
                Description = entity.Description,
                Length = entity.Length
            };
            return response;
        }

        public async Task<IEnumerable<BookListItem>> GetAllBooksAsync()
        {
            List<BookListItem> books = await _dbContext.Books
                .Select(entity => new BookListItem
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Author = entity.Author,
                    Description = entity.Description
                }).ToListAsync();
            return books;
        }

        public async Task<BookDetail?> GetBookByIdAsync(int bookId)
        {
            BookEntity? book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            return book is null ? null : new BookDetail
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Length = book.Length
            };
        }

    // 4/5 Private functions
    // CheckForACompleteBook -- Checks if the Supplied Data would complete a book in the DB, So Title, Description, author
    // FetchDataFromAPI(title, author) -- Search, Get the work
    // ParseWorkData(JsonData)
    // CreateAndLinkSubjects ()
    // CheckSubjectUnique(subject)
    }
}