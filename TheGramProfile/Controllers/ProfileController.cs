using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TheGramProfile.Domain.Commands.CreateFirebaseUserAndProfile;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.Query.GetProfile;

namespace TheGramProfile.Controllers
{
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest request)
        {
            var result = await _mediator.Send(new CreateFirebaseUserAndProfileCommand(request));
            if(result == null) return new ConflictResult();
            return new CreatedResult(result.Id,result);
        }      
        
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var result = await _mediator.Send(new GetProfileQuery(userId));
            if(result == null) return new NotFoundResult();
            return new OkObjectResult(result);
        }
        

        [HttpPut("{userId}")]
        [Authorize]
        public Task<ActionResult> UpdateProfile(string userId)
        {
            throw new NotImplementedException();
        }
    }
}