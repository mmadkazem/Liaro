namespace Liaro.Application.Account.Queries.LoginUser;


public class LoginUserQueryHandler : IRequestHandler<LoginUserQueryRequest, JwtTokensResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenFactoryService _tokenFactory;

    public LoginUserQueryHandler(IUnitOfWork uow,
            ITokenFactoryService tokenFactory)
    {
        _uow = uow;
        _tokenFactory = tokenFactory;
    }



    public async Task<JwtTokensResponse> Handle(LoginUserQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FindUserAsync(request.UserName, request.Password);
        if (user == null || !user.IsActive)
        {
            throw new UserNotExistByUserNameAndPasswordException();
        }

        var result = await _tokenFactory.CreateJwtTokensAsync(user);

        await _uow.UserTokens
                .AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);


        return new(result.AccessToken, result.RefreshToken);
    }
}