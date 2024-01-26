using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BW.Data;
using BW.Data.Entities;
using BW.Models.Book;
using BW.Models.OpenLibraryResponses;
using BW.Models.Subject;
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
            BookListItem? foundBook = CheckBookUnique(book.Title);
            if (foundBook != null)
                return foundBook;

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
                if (work == null)
                {
                    Console.WriteLine("no Books Found");
                    return null;
                }
                entity.Description = work.Description;
                // TODO: Update for book length
                _dbContext.Books.Add(entity);
                var numberOfChanges = await _dbContext.SaveChangesAsync();

                if (numberOfChanges != 1)
                {
                    Console.WriteLine("Book Didn't Actual Create");
                    return null;
                }
                Console.WriteLine("Should Have Created A book!");

                if (await CreateSubjects(work.Subjects, entity.Id) == false)
                {

                    _dbContext.Books.Remove(entity);
                    var numOfChanges = await _dbContext.SaveChangesAsync();
                    if (numOfChanges != 1)
                    {
                        return null;
                    }
                    return null;
                }
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
            List<BookSubjectEntity> bookToSubjects = _dbContext.BooksToSubjests.Where(entity => entity.BookId == bookId).ToList();
            List<string> subjects = new List<string>();
            foreach (BookSubjectEntity bookToSubject in bookToSubjects)
            {
                SubjectEntity? subject = await _dbContext.Subjects.FirstOrDefaultAsync(s => s.Id == bookToSubject.SubjectId);
                if (subject != null)
                {
                    subjects.Add(subject.Name);
                }
            }
            return book is null ? null : new BookDetail
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Length = book.Length,
                Subjects = subjects
            };
        }

        public async Task<IEnumerable<BookDetail>> GetBooksFromAuthorAsync(string author)
        {
            List<BookDetail> books = new List<BookDetail>();
            List<BookListItem> booksByAuthor = await _dbContext.Books
                .Where(b => b.Author == author)
                .Select(entity => new BookListItem
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Author = entity.Author,
                    Description = entity.Description
                }).ToListAsync();
            foreach (var book in booksByAuthor)
            {
                List<BookSubjectEntity> bookToSubjects = _dbContext.BooksToSubjests.Where(entity => entity.BookId == book.Id).ToList();
                List<string> subjects = new List<string>();
                foreach (BookSubjectEntity bookToSubject in bookToSubjects)
                {
                    SubjectEntity? subject = await _dbContext.Subjects.FirstOrDefaultAsync(s => s.Id == bookToSubject.SubjectId);
                    if (subject != null)
                    {
                        subjects.Add(subject.Name);
                    }
                }
                books.Add(new BookDetail
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Description = book.Description,
                    Length = book.Length,
                    Subjects = subjects
                });
            }
            return books;
        }

        public async Task<List<BookWithStars>> GetAllBooksByAVGRating(bool ascending)
        {
            List<BookWithStars> books = new List<BookWithStars>();

            List<BookListItem> bookListItems = await _dbContext.Books
                .Select(entity => new BookListItem
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Author = entity.Author,
                    Description = entity.Description
                }).ToListAsync();

            foreach(var book in bookListItems) {
                List<RatingEntity> ratings = _dbContext.Ratings.Where(rating => rating.BookId == book.Id).ToList();

                var avgStars = (int)ratings.Average(rating => rating.StarRating);
                books.Add(new BookWithStars(){
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Description = book.Description,
                    Length = book.Length,
                    StarRating = avgStars
                });
            }
            if(!ascending) {
                return books.OrderByDescending(entity => entity.StarRating).ToList();    
            }
            return books.OrderBy(entity => entity.StarRating).ToList();
        }

        public async Task<bool> AddSubjectToBook(AddSubjectToBook request)
        {
            SubjectEntity? subjectEntity = _dbContext.Subjects.Find(request.SubjectId);
            BookEntity? bookEntity = _dbContext.Books.Find(request.BookId);

            if (subjectEntity == null || bookEntity == null)
            {
                return false;
            }

            BookSubjectEntity entity = new()
            {
                BookId = bookEntity.Id,
                SubjectId = subjectEntity.Id
            };

            _dbContext.BooksToSubjests.Add(entity);
            var numOfChanges = await _dbContext.SaveChangesAsync();
            if (numOfChanges != 1)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveSubjectFromBook(AddSubjectToBook request)
        {
            SubjectEntity? subjectEntity = _dbContext.Subjects.Find(request.SubjectId);
            BookEntity? bookEntity = _dbContext.Books.Find(request.BookId);

            if (subjectEntity == null || bookEntity == null)
            {
                return false;
            }

            BookSubjectEntity entity = new()
            {
                BookId = bookEntity.Id,
                SubjectId = subjectEntity.Id
            };

            _dbContext.BooksToSubjests.Remove(entity);
            var numOfChanges = await _dbContext.SaveChangesAsync();
            if (numOfChanges != 1)
            {
                return false;
            }
            return true;
        }

        public async Task<List<BookDetail>> SearchForBookByTitle(BookSearch request)
        {
            List<BookDetail> books = await _dbContext.Books.Where(entity => entity.Title.ToLower().Contains(request.Title.ToLower()))
            .Select(entity => new BookDetail()
            {
                Id = entity.Id,
                Title = entity.Title,
                Author = entity.Author,
                Description = entity.Description,
                Length = entity.Length
            }).ToListAsync();

            return books;
        }

        public async Task<List<BookListItem>> GetBooksBySubjectAsync(string subject)
        {
            List<BookListItem> books = new List<BookListItem>();
            List<SubjectEntity> subjects = await _dbContext.Subjects.Where(s => s.Name.Contains(subject)).ToListAsync();
            foreach (var sub in subjects)
            {
                List<BookSubjectEntity> bookToSubjects = _dbContext.BooksToSubjests.Where(entity => entity.SubjectId == sub.Id).ToList();
                foreach (BookSubjectEntity bookToSubject in bookToSubjects)
                {
                    BookEntity? book = await _dbContext.Books.FirstOrDefaultAsync(s => s.Id == bookToSubject.BookId);
                    if (book != null && !books.Exists(b => b.Id == book.Id))
                    {
                        books.Add(new BookListItem
                        {
                            Id = book.Id,
                            Title = book.Title,
                            Author = book.Author,
                            Description = book.Description,
                            Length = book.Length
                        });
                    }
                }
            }
            return books;
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
        private async Task<OL_Works?> FetchDataFromAPI(string title, string author)
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
        private async Task<bool> CreateSubjects(List<string> subjects, int id)
        {
            foreach (string subject in subjects)
            {
                SubjectListItem? subjectEntity = new();
                subjectEntity = CheckSubjectUnique(subject);

                if (subjectEntity == null)
                {
                    SubjectEntity entity = new()
                    {
                        Name = subject
                    };

                    _dbContext.Subjects.Add(entity);

                    var numOfChanges = await _dbContext.SaveChangesAsync();

                    if (numOfChanges != 1)
                    {
                        return false;
                    }

                    subjectEntity = new SubjectListItem
                    {
                        Id = entity.Id,
                        Name = entity.Name
                    };
                }

                BookSubjectEntity bookSubjectEntity = new()
                {
                    BookId = id,
                    SubjectId = subjectEntity!.Id
                };

                _dbContext.BooksToSubjests.Add(bookSubjectEntity);

                var numberOfChanges = await _dbContext.SaveChangesAsync();

                if (numberOfChanges != 1)
                {
                    return false;
                }
            }
            return true;
        }

        // CheckSubjectUnique(subject)
        private SubjectListItem? CheckSubjectUnique(string subject)
        {
            List<SubjectListItem> entities = _dbContext.Subjects.Where(entity => entity.Name == subject)
            .Select(entity => new SubjectListItem()
            {
                Id = entity.Id,
                Name = entity.Name
            }).ToList();

            foreach (var entity in entities)
            {
                if (subject == entity.Name)
                {
                    return entity;
                }
            }

            return null;
        }


        private BookListItem? CheckBookUnique(string title)
        {
            List<BookListItem> entities = _dbContext.Books.Where(entity => entity.Title == title)
            .Select(entity => new BookListItem()
            {
                Id = entity.Id,
                Title = entity.Title,
                Author = entity.Author,
                Description = entity.Description,
                Length = entity.Length
            }).ToList();

            foreach (var entity in entities)
            {
                if (title == entity.Title)
                {
                    return entity;
                }
            }

            return null;
        }
    }
}