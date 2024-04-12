namespace Liaro.Application.Account.Queries.LoginByRefreshToken;


public record LoginByRefreshTokenQueryRequest(string RefreshToken)
    : IRequest<JwtTokensResponse>
{
    public static implicit operator LoginByRefreshTokenQueryRequest(string value)
        => new(value);

    public static implicit operator string(LoginByRefreshTokenQueryRequest request)
        => request.RefreshToken;
}