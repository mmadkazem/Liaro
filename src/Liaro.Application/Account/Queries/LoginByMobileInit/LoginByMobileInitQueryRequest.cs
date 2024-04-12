namespace Liaro.Application.Account.Queries.LoginByMobileInit;


public record LoginByMobileInitQueryRequest(string Code, string Mobile) : IRequest;