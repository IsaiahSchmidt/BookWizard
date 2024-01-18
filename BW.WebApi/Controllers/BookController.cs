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
    }
}
