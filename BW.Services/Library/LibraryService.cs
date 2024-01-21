using BW.Data;
using BW.Data.Entities;
using BW.Models.Book;
using BW.Models.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        foreach(LibraryEntity libraryEntity in  libraryEntities) {
            BookEntity? entity = _dbContext.Books.FirstOrDefault(entity => entity.Id == libraryEntity.BookId);
            if(entity != null) {
                BookDetail bookDetail = new(){
                    Id = entity.Id,
                    Title = entity.Title,
                    Author = entity.Author,
                    Description = entity.Description,
                    Length = entity.Length
                };

                bookEntities.Add(bookDetail);
            }
        }

        return bookEntities;
    }

    public async Task<bool> RemoveFromLibrary(LibraryRemove request)
    {
        LibraryEntity? entity = _dbContext.Libraries.FirstOrDefault(entity => entity.BookId == request.BookId && entity.UserId == _userId);
        
        if(entity is null) {
            return false;
        }

        _dbContext.Libraries.Remove(entity);
        
        var numberOfChanges = await _dbContext.SaveChangesAsync();

        if (numberOfChanges != 1)
            return false;

        return true;
    }
}
