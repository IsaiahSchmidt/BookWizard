using BW.Models.Rating;
using BW.Models.Responses;
using BW.Services.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BW.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRating([FromBody] RatingCreate rating)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var response = await _ratingService.CreateRatingAsync(rating);
            if(response is not null)
                return Ok(response);
            return BadRequest(new TextResponse("Could not create rating."));
            
        }

        [HttpGet("{ratingId:int}")]
        public async Task<IActionResult> GetRatingById([FromRoute] int ratingId)
        {
            RatingDetail? detail = await _ratingService.GetRatingByIdAsync(ratingId);
            return detail is not null ? Ok(detail) : NotFound();
        }

        [HttpGet("OwnerId/{ownerId:int}")]
        public async Task<IActionResult> GetRatingsByOwnerId([FromRoute] int ownerId)
        {
            var ratings = await _ratingService.GetRatingsByOwnerIdAsync(ownerId);
            return ratings is not null ? Ok(ratings) : NotFound();
        }
        
        [HttpGet("BookId/{bookId:int}")]
        public async Task<IActionResult> GetRatingsByBookId([FromRoute] int bookId)
        {
            var ratings = await _ratingService.GetRatingsByBookIdAsync(bookId);
            return ratings is not null ? Ok(ratings) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRating([FromBody] RatingUpdate rating)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return await _ratingService.UpdateRatingAsync(rating)
                ? Ok("Rating successfully updated")
                : BadRequest("Rating could not be updated");
        }

        [HttpDelete("{ratingId:int}")]
        public async Task<IActionResult> DeleteRating([FromRoute] int ratingId)
        {
            return await _ratingService.DeleteRatingAsync(ratingId)
                ? Ok($"Rating {ratingId} was successfully deleted")
                : BadRequest($"Rating {ratingId} could not be deleted");
        }
    }
}