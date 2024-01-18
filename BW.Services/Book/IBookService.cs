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
    }
}