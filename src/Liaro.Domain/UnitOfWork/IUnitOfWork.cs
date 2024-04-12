namespace Liaro.Domain.UnitOfWork;


public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IUserTokenRepository UserTokens { get; }
    IShortLinkRepository ShortLinks { get; }

    Task SaveChangeAsync();
}