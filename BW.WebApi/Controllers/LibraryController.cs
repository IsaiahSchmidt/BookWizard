using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BW.Models.Library;
using BW.Models.Responses;
using BW.Services.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BW.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {

        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBookToLibrary([FromBody] LibraryAdd request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _libraryService.AddToLibrary(request);
            if (response)
            {
                return Ok(new TextResponse("Book Added To Your Library"));
            }
            return BadRequest(new TextResponse("Could not add book to your Library"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks([FromQuery(Name = "showSubjects")] bool showSubjects = false)
        {
            var books = await _libraryService.GetAllBookInLibrary(showSubjects);
            return Ok(books);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBookFromLibrary([FromBody] LibraryRemove request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _libraryService.RemoveFromLibrary(request);
            if (response)
            {
                return Ok(new TextResponse("Book removed from your library"));
            }
            return BadRequest(new TextResponse("Could not remove book from your Library"));
        }

        [HttpGet("/Ratings")]
        public async Task<IActionResult> FilterLibraryByRating([FromQuery(Name = "ascending")] bool ascending = true)
        {
            var library = await _libraryService.FilterLibraryByRatingAsync(ascending);
            return Ok(library);
        }
    }
}
