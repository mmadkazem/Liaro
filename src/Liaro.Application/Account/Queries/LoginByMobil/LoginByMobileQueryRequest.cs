namespace Liaro.Application.Account.Queries.LoginByMobil;


public record LoginByMobileQueryRequest(string Mobile, string Code)
    : IRequest<JwtTokensResponse>;