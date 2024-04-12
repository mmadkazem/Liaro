namespace Liaro.Application.Account.Commands.UserLogout;


public sealed class UserLogoutCommandHandler : IRequestHandler<UserLogoutCommandRequest>
{
    private readonly IUnitOfWork _uow;
    private readonly ISecurityService _securityService;
    private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
    private readonly ITokenFactoryService _tokenFactory;

    public UserLogoutCommandHandler(IUnitOfWork uow,
        ISecurityService securityService,
        IOptionsSnapshot<BearerTokensOptions> configuration,
        ITokenFactoryService tokenFactory)
    {
        _uow = uow;
        _securityService = securityService;
        _configuration = configuration;
        _tokenFactory = tokenFactory;
    }

    public async Task Handle(UserLogoutCommandRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.UserId) && int.TryParse(request.UserId, out int userId))
        {
            if (_configuration.Value.AllowSignOutAllUserActiveClients)
            {
                await _uow.UserTokens.InvalidateUserTokensAsync(userId);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            var refreshTokenSerial = _tokenFactory.GetRefreshTokenSerial(request.RefreshToken);
            if (!string.IsNullOrWhiteSpace(refreshTokenSerial))
            {
                var refreshTokenIdHashSource = _securityService.GetSha256Hash(refreshTokenSerial);
                await _uow.UserTokens.DeleteTokensWithSameRefreshTokenSourceAsync(refreshTokenIdHashSource);
            }
        }

        await _uow.UserTokens.DeleteExpiredTokensAsync();
        await _uow.SaveChangeAsync();
    }
}