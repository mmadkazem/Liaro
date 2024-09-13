namespace Liaro.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class AccountController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommandRequest request)
    {
        await _sender.Send(request);
        return Ok();
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginUserQueryRequest request)
    {
        var result = await _sender.Send(request);
        return Ok(result);

    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginByMobileInit
        ([FromBody] LoginByMobileInitQueryRequest request)
    {
        await _sender.Send(request);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LoginByMobile
        ([FromBody] LoginByMobileQueryRequest request)
    {
        var result = await _sender.Send(request);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] JToken jsonBody)
    {
        LoginByRefreshTokenQueryRequest request = jsonBody.Value<string>("refreshToken");
        var result = await _sender.Send(request);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Logout(string refreshToken)
    {
        await _sender.Send(new UserLogoutCommandRequest(User.UserId(), refreshToken));
        return Ok();
    }

    [HttpGet]
    public bool? IsAuthenticated()
    {
        return User.Identity?.IsAuthenticated;
    }

    [HttpGet]
    public IActionResult GetUserInfo()
    {
        return Ok(new
        {
            UserId = User.UserId(),
            Username = User.UserName(),
            Roles = User.Roles()
        });
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword
        ([FromBody] ChangePasswordDTO model)
    {
        var request = ChangePasswordCommandRequest.Create(User.UserId(), model);
        await _sender.Send(request);
        return Ok();
    }
}