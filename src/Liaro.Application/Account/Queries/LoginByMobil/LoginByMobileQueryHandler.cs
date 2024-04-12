namespace Liaro.Application.Account.Queries.LoginByMobil;


public class LoginByMobileQueryHandler
    : IRequestHandler<LoginByMobileQueryRequest, JwtTokensResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenFactoryService _tokenFactory;

    public LoginByMobileQueryHandler(IUnitOfWork uow, ITokenFactoryService tokenFactory)
    {
        _uow = uow;
        _tokenFactory = tokenFactory;
    }

    public async Task<JwtTokensResponse> Handle(LoginByMobileQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FindByMobileAndLoginCode(request.Mobile, request.Code);
        if (user is null || !user.IsActive)
        {
            throw new InvalidLoginCodeException();
        }
        if (user.MobileLoginExpire != null && user.MobileLoginExpire < DateTimeOffset.UtcNow)
        {
            throw new LoginCodeExpireException();
        }

        user.MobileLoginExpire = null;
        user.LoginCode = null;

        _uow.Users.Update(user);

        var result = await _tokenFactory.CreateJwtTokensAsync(user);

        await _uow.UserTokens
                .AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);

        await _uow.SaveChangeAsync();

        return new(result.AccessToken, result.RefreshToken);
    }
}