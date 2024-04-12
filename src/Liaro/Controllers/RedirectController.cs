namespace Liaro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedirectController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [Authorize(Policy = CustomRoles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShortlinkCommandRequest request)
    {
        request.UserId = User.UserId();
        var result = await _sender.Send(request);
        return Ok(result);
    }

    [Authorize(Policy = CustomRoles.Admin)]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateShortLinkCommandRequest request)
    {
        await _sender.Send(request);
        return Ok();
    }

    [Authorize(Policy = CustomRoles.Admin)]
    [HttpDelete("{source}")]
    public async Task<IActionResult> Remove(RemoveShortLinkCommandRequest request)
    {
        await _sender.Send(request);
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("/t/{source}")]
    public async Task<IActionResult> RedirectOtherId(RedirectOtherIdCommandRequest request)
    {
        var result = await _sender.Send(request);
        return Redirect(result);
    }

}