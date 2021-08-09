using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //Below is an action Filter used for tracking Last Active User
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}