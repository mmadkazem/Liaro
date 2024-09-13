namespace Liaro.Domain.Repository;


public interface IUserTokenRepository
{
    Task AddUserTokenAsync(User? user, string refreshTokenSerial, string accessToken, string? refreshTokenSourceSerial, CancellationToken token);
    Task<bool> IsValidTokenAsync(string accessToken, int userId, CancellationToken token);
    Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource, CancellationToken token);
    Task DeleteExpiredTokensAsync(CancellationToken token);
    Task InvalidateUserTokensAsync(int userId, CancellationToken token);
    Task<UserToken> FindTokenAsync(string refreshToken, CancellationToken token);
}