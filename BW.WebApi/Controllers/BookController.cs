using BW.Models.Book;
using BW.Models.Responses;
using BW.Services.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BW.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateBook([FromBody] BookCreate book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _bookService.CreateBookAsync(book);
            if (response is not null)
            {
                return Ok(response);
            }
            return BadRequest(new TextResponse("Could not create book"));
        }


        [HttpPost("Subject")]
        public async Task<IActionResult> AddSubjectToBook([FromBody] AddSubjectToBook request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _bookService.AddSubjectToBook(request);
            if (response)
            {
                return Ok("Subject added!");
            }
            return BadRequest(new TextResponse("Could not add subject to book"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{bookId:int}")]
        public async Task<IActionResult> GetBookById([FromRoute] int bookId)
        {
            BookDetail? book = await _bookService.GetBookByIdAsync(bookId);
            return book is not null ? Ok(book) : NotFound();
        }

        [HttpDelete("Subject")]
        public async Task<IActionResult> RemoveSubjectFromBook([FromBody] AddSubjectToBook request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _bookService.RemoveSubjectFromBook(request);
            if (response)
            {
                return Ok("Subject removed!");
            }
            return BadRequest(new TextResponse("Could not remove subject from book"));
        }
    }
}
