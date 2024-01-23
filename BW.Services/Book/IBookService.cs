using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BW.Models.Book;

namespace BW.Services.Book
{
    public interface IBookService
    {
        Task<BookListItem?> CreateBookAsync(BookCreate book);
        Task<IEnumerable<BookListItem>> GetAllBooksAsync();
        Task<BookDetail?> GetBookByIdAsync(int bookId);
        Task<IEnumerable<BookListItem>> GetBooksFromAuthorAsync(string author);
        Task<List<BookDetail>> SearchForBookByTitle(BookSearch request);
        Task<bool> AddSubjectToBook(AddSubjectToBook request);
        Task<bool> RemoveSubjectFromBook(AddSubjectToBook request);
    }
}