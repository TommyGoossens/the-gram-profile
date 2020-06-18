using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TheGramProfile.Domain.Commands.UpdateFollower;
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
            var result = await _mediator.Send(new UpdateFollowerCommand(request));
            if (!result)
            {
                return new BadRequestResult();
            }

            return new OkResult();
        }
    }
}