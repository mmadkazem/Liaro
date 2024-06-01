
namespace Liaro.Application.Account.Queries.LoginUser;


public sealed class LoginUserQueryHandler(IUnitOfWork uow, ITokenFactoryService tokenFactory)
    : IRequestHandler<LoginUserQueryRequest, JwtTokensResponse>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly ITokenFactoryService _tokenFactory = tokenFactory;

    public async Task<JwtTokensResponse> Handle(LoginUserQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FindUserAsync(request.UserName, request.Password);
        if (user is null)
        {
            throw new UserNotExistByUserNameAndPasswordException();
        }

        var token = await _tokenFactory.CreateJwtTokensAsync(user);

        await _uow.UserTokens.AddUserTokenAsync(user, token.RefreshTokenSerial, token.AccessToken, null);

        return new (token.AccessToken, token.RefreshToken);
    }
}