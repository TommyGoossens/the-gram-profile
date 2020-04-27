using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheGramProfile.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class AbstractProfileController : ControllerBase
    {
    }
}