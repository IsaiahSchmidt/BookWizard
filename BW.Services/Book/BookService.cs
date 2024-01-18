using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BW.Data;
using BW.Data.Entities;
using BW.Models.Book;
using BW.Models.OpenLibraryResponses;
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
            BookEntity entity = new();
            if (CheckForACompleteBook(book))
            {
                entity.Title = book.Title;
                entity.Author = book.Author;
                entity.Description = book.Description;
                entity.Length = book.Length;

                _dbContext.Books.Add(entity);
                var numberOfChanges = await _dbContext.SaveChangesAsync();

                if (numberOfChanges != 1)
                {
                    return null;
                }
            }
            else
            {
                entity.Title = book.Title;
                entity.Author = book.Author;

                OL_Works? work = await FetchDataFromAPI(book.Title, book.Author);
                if(work == null)
                    return null;
                entity.Description = work.Description;
                 _dbContext.Books.Add(entity);
                var numberOfChanges = await _dbContext.SaveChangesAsync();

                if (numberOfChanges != 1)
                {
                    return null;
                }
                // CreateSubjects(work.Subjects, entity.Id);
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

        //* Helper Methods

        // 4/5 Private functions
        // CheckForACompleteBook -- Checks if the Supplied Data would complete a book in the DB, So Title, Description, author
        private bool CheckForACompleteBook(BookCreate request)
        {
            if (request.Description == string.Empty || request.Description == null)
                return false;
            return true;
        }
        // FetchDataFromAPI(title, author) -- Search, Get the work
        private async Task<OL_Works> FetchDataFromAPI(string title, string author)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage searchResponse = await client.GetAsync($"https://openlibrary.org/search.json?title={title}&author={author}&language=eng");
            OL_SearchResponse? oL_Search = JsonSerializer.Deserialize<OL_SearchResponse>(await searchResponse.Content.ReadAsStringAsync());
            if (oL_Search == null)
                return null;
            if (oL_Search.Docs.Count < 1)
                return null;
            HttpResponseMessage worksResponse = await client.GetAsync($"https://openlibrary.org{oL_Search.Docs[0].Key}.json");

            OL_Works? oL_Works = JsonSerializer.Deserialize<OL_Works>(await worksResponse.Content.ReadAsStringAsync());
            if (oL_Works == null)
                return null;
            return oL_Works;

        }
        // CreateAndLinkSubjects ()
        private void CreateSubjects(List<string> subjects, int id)
        {
            foreach (string subject in subjects)
            {
                CheckSubjectUnique(subject);
            }
        }

        // CheckSubjectUnique(subject)
        private void CheckSubjectUnique(string subject)
        {
        //!    SubjectEntity? entity = _dbContext.Subjects.Select(entity => entity.Name).Where(Name => Name == subject)
        }
    }
}