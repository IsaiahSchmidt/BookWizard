using BW.Data;
using BW.Data.Entities;
using BW.Models.Book;
using BW.Models.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BW.Services.Library;

public class LibraryService : ILibraryService
{

    private readonly ApplicationDbContext _dbContext;
    private readonly int _userId;

    public LibraryService(UserManager<UserEntity> userManager,
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

    public async Task<bool> AddToLibrary(LibraryAdd request)
    {
        LibraryEntity entity = new()
        {
            BookId = request.BookId,
            UserId = _userId
        };

        _dbContext.Libraries.Add(entity);

        var numberOfChanges = await _dbContext.SaveChangesAsync();

        if (numberOfChanges != 1)
            return false;

        return true;
    }

    public async Task<List<BookDetail>> GetAllBookInLibrary()
    {
        List<LibraryEntity> libraryEntities = await _dbContext.Libraries.Where(entity => entity.UserId == _userId).ToListAsync();
        List<BookDetail> bookEntities = new List<BookDetail>();

        foreach (LibraryEntity libraryEntity in libraryEntities)
        {
            BookEntity? entity = _dbContext.Books.FirstOrDefault(entity => entity.Id == libraryEntity.BookId);
            if (entity != null)
            {
                BookDetail bookDetail = new()
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Author = entity.Author,
                    Description = entity.Description,
                    Length = entity.Length
                };

                bookEntities.Add(bookDetail);
            }
        }
        List<BookDetail> books = new List<BookDetail>();
        foreach (var book in bookEntities)
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

    public async Task<bool> RemoveFromLibrary(LibraryRemove request)
    {
        LibraryEntity? entity = _dbContext.Libraries.FirstOrDefault(entity => entity.BookId == request.BookId && entity.UserId == _userId);

        if (entity is null)
        {
            return false;
        }

        _dbContext.Libraries.Remove(entity);

        var numberOfChanges = await _dbContext.SaveChangesAsync();

        if (numberOfChanges != 1)
            return false;

        return true;
    }

    public async Task<List<BookWithStars>> FilterLibraryByRatingAsync(bool ascending)
    {
        List<LibraryEntity> libraryEntities = await _dbContext.Libraries.Where(entity => entity.UserId == _userId).ToListAsync();
        List<BookWithStars> bookEntities = new List<BookWithStars>();

        foreach (LibraryEntity libraryEntity in libraryEntities)
        {
            RatingEntity? rating = await _dbContext.Ratings.FirstOrDefaultAsync(entity => entity.BookId == libraryEntity.BookId &&
                entity.OwnerId == _userId);
            BookEntity? entity = _dbContext.Books.FirstOrDefault(entity => entity.Id == libraryEntity.BookId);
            if (entity != null && rating != null)
            {
                BookWithStars bookDetail = new()
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Author = entity.Author,
                    Description = entity.Description,
                    Length = entity.Length,
                    StarRating = rating.StarRating
                };

                bookEntities.Add(bookDetail);
            }
        }
        if (!ascending)
            return bookEntities.OrderByDescending(entity => entity.StarRating).ToList();
        return bookEntities.OrderBy(entity => entity.StarRating).ToList();
    }
}
