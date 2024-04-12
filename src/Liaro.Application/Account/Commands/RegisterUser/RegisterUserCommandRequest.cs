namespace Liaro.Application.Account.Commands.RegisterUser;


public record RegisterUserCommandRequest
(
    string UserName,
    string Password,
    string Email,
    string Mobile,
    string DisplayName
) : IRequest;