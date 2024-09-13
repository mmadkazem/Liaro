namespace Liaro.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RedirectController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> Create(CreateShortlinkDTO model,
        CancellationToken token)
    {
        var request = CreateShortlinkCommandRequest.Create(User.UserId(), model);
        var result = await _sender.Send(request, token);
        return Ok(result);
    }

    [HttpPut("{id:int:required}")]
    public async Task<IActionResult> Update(int id, UpdateShortLinkDTO model,
        CancellationToken token)
    {
        var request = UpdateShortLinkCommandRequest.Create(id, model);
        await _sender.Send(request, token);
        return Ok();
    }

    [HttpDelete("{source}")]
    public async Task<IActionResult> Remove(string source,
        CancellationToken token)
    {
        await _sender.Send(new RemoveShortLinkCommandRequest(source), token);
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("/t/{source}")]
    public async Task<IActionResult> RedirectOtherId(string source,
        CancellationToken token)
    {
        var result = await _sender.Send(new RedirectOtherIdCommandRequest(source), token);
        return Redirect(result);
    }

}