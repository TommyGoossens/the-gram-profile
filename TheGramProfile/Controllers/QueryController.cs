using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.Query.SearchPaginatedProfiles;

namespace TheGramProfile.Controllers
{
    [Route("api/profile/query")]
    [Authorize]
    public class QueryController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMediator _mediator;

        public QueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public string Test()
        {
            Logger.Info("Working fine");
            return "working fine";
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> QueryProfiles([FromBody] QueryProfilesRequest request)
        {
            var result = await _mediator.Send(new SearchPaginatedProfilesQuery(request));
            if (result == null) return new NotFoundResult();
            return new OkObjectResult(result);
        }
    }
}