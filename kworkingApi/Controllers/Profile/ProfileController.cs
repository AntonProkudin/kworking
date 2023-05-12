using kworkingApi.Controllers.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kworkingApi.Controllers.Profile;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProfileController : Controller
{
    IUserFunction _userFunction;

    public ProfileController(IUserFunction userFunction, IUserFriendFunction userFriendFunction, IMessageFunction messageFunction)
    {
        _userFunction = userFunction;
    }

    [HttpPost("Initialize")]
    public async Task<ActionResult> Initialize([FromBody]int userId)
    {
        var response = new ProfileInitializeResponse
        {
            User = _userFunction.GetUserById(userId),
        };

        return Ok(response);
    }
    [HttpPost("PutPhoto")]
    public async Task<ActionResult> PutPhoto([FromBody] PutPhotoRequest request)
    {
        _userFunction.PutPhotoById(request.FromUserId, request.Path);
         var response = 200;

        return Ok(response);
    }
}
