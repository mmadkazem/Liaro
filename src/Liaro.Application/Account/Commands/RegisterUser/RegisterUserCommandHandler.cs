namespace Liaro.Application.Account.Commands.RegisterUser;


public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest>
{
    private readonly IUnitOfWork _uow;
    private readonly ISecurityService _securityService;

    public RegisterUserCommandHandler(IUnitOfWork uow,
                ISecurityService securityService)
    {
        _uow = uow;
        _securityService = securityService;
    }

    public async Task Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
    {
        User user = new()
        {
            Username = request.UserName,
            Email = request.Email,
            Mobile = request.Mobile,
            Password = _securityService.GetSha256Hash(request.Password),
            DisplayName = request.DisplayName,
            IsActive = true
        };

        _uow.Users.Add(user);
        await _uow.SaveChangeAsync();
    }
}