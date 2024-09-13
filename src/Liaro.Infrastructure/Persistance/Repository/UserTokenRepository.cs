namespace Liaro.Infrastructure.Persistance.Repository;


public class UserTokenRepository : IUserTokenRepository
{
    private readonly ISecurityService _securityService;
    private readonly ApplicationDbContext _context;
    private readonly IOptions<BearerTokensOptions> _configuration;
    public UserTokenRepository(
        ISecurityService securityService,
        IOptions<BearerTokensOptions> configuration,
        ApplicationDbContext context)
    {

        _securityService = securityService;
        _securityService.CheckArgumentIsNull(nameof(_securityService));

        // _tokens = tokens;

        _configuration = configuration;
        _configuration.CheckArgumentIsNull(nameof(configuration));
        _context = context;
    }

    private async Task AddUserTokenAsync(UserToken userToken, CancellationToken token)
    {
        if (!_configuration.Value.AllowMultipleLoginsFromTheSameUser)
        {
            await InvalidateUserTokensAsync(userToken.UserId, token);
        }
        await DeleteTokensWithSameRefreshTokenSourceAsync(userToken.RefreshTokenIdHashSource, token);
        _context.UserTokens.Add(userToken);
    }

    public async Task AddUserTokenAsync(User user, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var token = new UserToken
        {
            UserId = user.Id,
            // Refresh token handles should be treated as secrets and should be stored hashed
            RefreshTokenIdHash = _securityService.GetSha256Hash(refreshTokenSerial),
            RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSourceSerial) ?
                                        null : _securityService.GetSha256Hash(refreshTokenSourceSerial),
            AccessTokenHash = _securityService.GetSha256Hash(accessToken),
            RefreshTokenExpiresDateTime = now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
            AccessTokenExpiresDateTime = now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes)
        };
        await AddUserTokenAsync(token, cancellationToken);
    }

    public async Task DeleteExpiredTokensAsync(CancellationToken token)
        => await _context.UserTokens
                        .AsQueryable()
                        .Where(x => x.RefreshTokenExpiresDateTime < DateTimeOffset.UtcNow)
                        .ExecuteDeleteAsync(token);

    public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource, CancellationToken token)
        => await _context.UserTokens
                    .AsQueryable()
                    .Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource)
                    .ExecuteDeleteAsync(token);


    public async Task InvalidateUserTokensAsync(int userId, CancellationToken token)
        => await _context.UserTokens
                    .Where(x => x.UserId == userId)
                    .ExecuteDeleteAsync(token);


    public async Task<bool> IsValidTokenAsync(string accessToken, int userId, CancellationToken token)
    {
        var accessTokenHash = _securityService.GetSha256Hash(accessToken);

        var userToken = await _context.UserTokens
                .AsQueryable()
                .Where(x => x.AccessTokenHash == accessTokenHash && x.UserId == userId)
                .FirstOrDefaultAsync(token);

        return userToken?.AccessTokenExpiresDateTime >= DateTimeOffset.UtcNow;
    }

    public async Task<UserToken> FindTokenAsync(string refreshToken, CancellationToken token)
    {
        var refreshTokenSerial = _securityService.GetRefreshTokenSerial(refreshToken);
        if (string.IsNullOrWhiteSpace(refreshTokenSerial))
        {
            return null;
        }

        var refreshTokenIdHash = _securityService.GetSha256Hash(refreshTokenSerial);
        return await _context.UserTokens.AsQueryable()
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash, token);
    }
}