namespace Liaro.Application.Account.Queries.LoginByMobileInit;


public sealed class LoginByMobileInitQueryHandler : IRequestHandler<LoginByMobileInitQueryRequest>
{
    private readonly IUnitOfWork _uow;
    private readonly IKavenegarService _kavenegarService;

    public LoginByMobileInitQueryHandler(IUnitOfWork uow,
            IKavenegarService kavenegarService)
    {
        _uow = uow;
        _kavenegarService = kavenegarService;
    }

    public async Task Handle(LoginByMobileInitQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FindAsyncByMobileAsync(request.Mobile);

        if (user is null || !user.IsActive)
        {
            throw new UserByMobileNotExistException();
        }
        if (user.MobileLoginExpire != null && user.MobileLoginExpire > DateTimeOffset.UtcNow)
        {
            throw new LoginTimeExpireException();
        }

        user.MobileLoginExpire = DateTimeOffset.UtcNow.AddMinutes(5);
        user.LoginCode = StringUtils.GetUniqueKey(6);

        var res = await _kavenegarService
            .SendLoginCode(user.LoginCode, request.Mobile, user.DisplayName) ?? throw new KavenegarNoResponseException();

        if (res.@return?.status != 200)
        {
            throw new KavenegarNoResponseException();
        }
        _uow.Users.Update(user);
        await _uow.SaveChangeAsync();
    }
}