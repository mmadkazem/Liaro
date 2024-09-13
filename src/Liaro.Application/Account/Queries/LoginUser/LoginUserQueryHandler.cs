namespace Liaro.Application.Account.Queries.LoginUser;


public sealed class LoginUserQueryHandler(IUnitOfWork uow, ITokenFactoryService tokenFactory)
    : IRequestHandler<LoginUserQueryRequest, JwtTokensResponse>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly ITokenFactoryService _tokenFactory = tokenFactory;

    public async Task<JwtTokensResponse> Handle(LoginUserQueryRequest request, CancellationToken token)
    {
        var user = await _uow.Users.FindUserAsync(request.UserName, request.Password, token)
            ?? throw new UserNotExistByUserNameAndPasswordException();

        var userToken = await _tokenFactory.CreateJwtTokensAsync(user);

        await _uow.UserTokens.AddUserTokenAsync(user, userToken.RefreshTokenSerial, userToken.AccessToken, null, token);
        return new (userToken.AccessToken, userToken.RefreshToken);
    }
}