using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TheGramProfile.Domain.Commands.UpdateFollowerForUser;
using TheGramProfile.Domain.DTO.Request;

namespace TheGramProfile.Controllers
{
    [Route("api/profile/follower")]
    public class FollowerController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMediator _mediator;

        public FollowerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFollowerForUser([FromBody] UpdateFollowerRequest request)
        {
            var result = await _mediator.Send(new UpdateFollowerForUserCommand(request.UserId,request.UserIdToFollow));
            return new OkResult();
        }
    }
}