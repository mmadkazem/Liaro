namespace Liaro.Domain.Repository;


public interface IUserTokenRepository
{
    Task AddUserTokenAsync(User? user, string refreshTokenSerial, string accessToken, string? refreshTokenSourceSerial);
    Task<bool> IsValidTokenAsync(string accessToken, int userId);
    Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource);
    Task DeleteExpiredTokensAsync();
    Task InvalidateUserTokensAsync(int userId);
    Task<UserToken> FindTokenAsync(string refreshToken);
}