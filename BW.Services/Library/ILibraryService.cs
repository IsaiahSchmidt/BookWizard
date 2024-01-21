using BW.Models.Book;
using BW.Models.Library;

namespace BW.Services.Library;

public interface ILibraryService
{
    public Task<bool> AddToLibrary(LibraryAdd request);

    public Task<bool> RemoveFromLibrary(LibraryRemove request);

    public Task<List<BookDetail>> GetAllBookInLibrary();
}
