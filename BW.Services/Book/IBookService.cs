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
        Task<IEnumerable<BookDetail>> GetAllBooksAsync(bool subjects);
        Task<BookDetail?> GetBookByIdAsync(int bookId);
        Task<IEnumerable<BookDetail>> GetBooksFromAuthorAsync(string author, bool showSubjects);
        Task<List<BookDetail>> SearchForBookByTitle(BookSearch request);
        Task<bool> AddSubjectToBook(AddSubjectToBook request);
        Task<bool> RemoveSubjectFromBook(AddSubjectToBook request);
        Task<List<BookListItem>> GetBooksBySubjectAsync(string subject); 
        Task<List<BookWithStarsDouble>> GetAllBooksByAVGRating(bool ascending);
    }
}