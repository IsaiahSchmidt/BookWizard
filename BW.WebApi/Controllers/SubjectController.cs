using BW.Models.Responses;
using BW.Models.Subject;
using BW.Services.Subject;
using Microsoft.AspNetCore.Mvc;

namespace BW.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpPost("Create")]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectCreate subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _subjectService.CreateSubject(subject);
            if (response is not null)
            {
                return Ok(response);
            }
            return BadRequest(new TextResponse("Could not create book"));
        }
}

