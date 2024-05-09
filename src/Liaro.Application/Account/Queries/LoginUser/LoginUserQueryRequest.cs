namespace Liaro.Application.Account.Queries.LoginUser;

public record LoginUserQueryRequest(string UserName, string Password)
     : IRequest<JwtTokensResponse>;