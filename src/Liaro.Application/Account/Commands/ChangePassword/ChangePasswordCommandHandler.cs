
namespace Liaro.Application.Account.Commands.ChangePassword;


public sealed class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommandRequest>
{
    private readonly IUnitOfWork _uow;
    private readonly ISecurityService _securityService;

    public ChangePasswordCommandHandler(IUnitOfWork uow, ISecurityService securityService)
    {
        _uow = uow;
        _securityService = securityService;
    }

    public async Task Handle(ChangePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FindAsync(request.UserId);
        var currentPasswordHash = _securityService.GetSha256Hash(request.OldPassword);
        if (user.Password != currentPasswordHash)
        {
            throw new NewPasswordNotEqualException();
        }

        user.Password = _securityService.GetSha256Hash(request.NewPassword);
        user.SerialNumber = Guid.NewGuid().ToString("N"); // To force other logins to expire.
        _uow.Users.Update(user);
        await _uow.SaveChangeAsync();
    }
}