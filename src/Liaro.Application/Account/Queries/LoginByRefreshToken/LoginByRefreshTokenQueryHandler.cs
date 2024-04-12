namespace Liaro.Application.Account.Queries.LoginByRefreshToken;

public sealed class LoginByRefreshTokenQueryHandler
    : IRequestHandler<LoginByRefreshTokenQueryRequest, JwtTokensResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenFactoryService _tokenFactory;

    public LoginByRefreshTokenQueryHandler(IUnitOfWork uow,
        ITokenFactoryService tokenFactory)
    {
        _uow = uow;
        _tokenFactory = tokenFactory;
    }

    public async Task<JwtTokensResponse> Handle(LoginByRefreshTokenQueryRequest request, CancellationToken cancellationToken)
    {
        var token = await _uow.UserTokens.FindTokenAsync(request);
        if (token is null)
        {
            throw new UserNotExistException();
        }

        var result = await _tokenFactory.CreateJwtTokensAsync(token.User);
        await _uow.UserTokens
                .AddUserTokenAsync(token.User, result.RefreshTokenSerial, result.AccessToken,
                _tokenFactory.GetRefreshTokenSerial(request.RefreshToken));

        return new(result.AccessToken, result.RefreshToken);
    }
}