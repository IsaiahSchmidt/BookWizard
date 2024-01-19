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
    }
}