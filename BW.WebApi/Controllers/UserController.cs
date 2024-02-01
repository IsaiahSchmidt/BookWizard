using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BW.Models.Responses;
using BW.Models.Token;
using BW.Models.User;
using BW.Services.Token;
using BW.Services.User;

namespace BW.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _userService.RegisterUserAsync(model);
            if (registerResult)
            {
                TextResponse response = new("User was registered");
                return Ok(response);
            }
            return BadRequest(new TextResponse("User could not be registered"));
        }

        [HttpPost("~/api/Token")]
        public async Task<IActionResult> GetToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TokenResponse? response = await _tokenService.GetTokenAsync(request);

            if (response is null)
                return BadRequest(new TextResponse("Invalid username or password"));

            return Ok(response);
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            UserDetail? detail = await _userService.GetUserByIdAsync(userId);
            if (detail is null)
            {
                return NotFound();
            }
            return Ok(detail);
        }

        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId) {
            return await _userService.DeleteUserAsync(userId) ? Ok($"User {userId} was deleted successfully") : BadRequest($"User {userId} was unable to be deleted!");
        }
    }
}
