namespace Liaro.Application.Account.Commands.UserLogout;


public sealed class UserLogoutCommandHandler(IUnitOfWork uow,
        ISecurityService securityService,
        IOptions<BearerTokensOptions> configuration
    ) : IRequestHandler<UserLogoutCommandRequest>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly ISecurityService _securityService = securityService;
    private readonly IOptions<BearerTokensOptions> _configuration = configuration;

    public async Task Handle(UserLogoutCommandRequest request, CancellationToken token)
    {

        if (_configuration.Value.AllowSignOutAllUserActiveClients)
        {
            await _uow.UserTokens.InvalidateUserTokensAsync(request.UserId, token);
        }
        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            var refreshTokenSerial = _securityService.GetRefreshTokenSerial(request.RefreshToken);
            if (!string.IsNullOrWhiteSpace(refreshTokenSerial))
            {
                var refreshTokenIdHashSource = _securityService.GetSha256Hash(refreshTokenSerial);
                await _uow.UserTokens.DeleteTokensWithSameRefreshTokenSourceAsync(refreshTokenIdHashSource, token);
            }
        }

        await _uow.UserTokens.DeleteExpiredTokensAsync(token);
        await _uow.SaveChangeAsync(token);
    }
}