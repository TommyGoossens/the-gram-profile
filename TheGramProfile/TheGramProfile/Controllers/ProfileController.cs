using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TheGramProfile.Domain.Query.GetProfile;

namespace TheGramProfile.Controllers
{
    public class ProfileController : AbstractProfileController
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var result = await _mediator.Send(new GetProfileQuery(userId));
            if(result == null) return new NotFoundResult();
            return new OkObjectResult(result);
        }

        [HttpPut("{userId}")]
        public Task<ActionResult> UpdateProfile(string userId)
        {
            throw new NotImplementedException();
        }
    }
}