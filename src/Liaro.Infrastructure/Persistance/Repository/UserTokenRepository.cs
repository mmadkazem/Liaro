namespace Liaro.Infrastructure.Persistance.Repository;


public class UserTokenRepository : IUserTokenRepository
{
    private readonly ISecurityService _securityService;
    private readonly ApplicationDbContext _context;
    private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
    private readonly ITokenFactoryService _tokenFactoryService;

    public UserTokenRepository(
        IEntityBaseRepository<UserToken> tokens,
        ISecurityService securityService,
        IOptionsSnapshot<BearerTokensOptions> configuration,
        ITokenFactoryService tokenFactoryService,
        ApplicationDbContext context)
    {

        _securityService = securityService;
        _securityService.CheckArgumentIsNull(nameof(_securityService));

        // _tokens = tokens;

        _configuration = configuration;
        _configuration.CheckArgumentIsNull(nameof(configuration));

        _tokenFactoryService = tokenFactoryService;
        _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));
        _context = context;
    }

    private async Task AddUserTokenAsync(UserToken userToken)
    {
        if (!_configuration.Value.AllowMultipleLoginsFromTheSameUser)
        {
            await InvalidateUserTokensAsync(userToken.UserId);
        }
        await DeleteTokensWithSameRefreshTokenSourceAsync(userToken.RefreshTokenIdHashSource);
        _context.UserTokens.Add(userToken);
    }

    public async Task AddUserTokenAsync(User user, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial)
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
        await AddUserTokenAsync(token);
    }

    public async Task DeleteExpiredTokensAsync()
        => await _context.UserTokens
                        .AsQueryable()
                        .Where(x => x.RefreshTokenExpiresDateTime < DateTimeOffset.UtcNow)
                        .ForEachAsync(userToken =>
                        {
                            _context.UserTokens.Remove(userToken);
                        });

    public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource)
        => await _context.UserTokens
                    .AsQueryable()
                    .Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource)
                    .ForEachAsync(userToken =>
                    {
                        _context.UserTokens.Remove(userToken);
                    });


    public async Task InvalidateUserTokensAsync(int userId)
        => await _context.UserTokens
                    .Where(x => x.UserId == userId)
                    .ForEachAsync(userToken =>
                    {
                        _context.UserTokens.Remove(userToken);
                    });


    public async Task<bool> IsValidTokenAsync(string accessToken, int userId)
    {
        var accessTokenHash = _securityService.GetSha256Hash(accessToken);

        var userToken = await _context.UserTokens
                .AsQueryable()
                .Where(x => x.AccessTokenHash == accessTokenHash && x.UserId == userId)
                .FirstOrDefaultAsync();

        return userToken?.AccessTokenExpiresDateTime >= DateTimeOffset.UtcNow;
    }

    public async Task<UserToken> FindTokenAsync(string refreshToken)
    {
        var refreshTokenSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshToken);
        if (string.IsNullOrWhiteSpace(refreshTokenSerial))
        {
            return null;
        }

        var refreshTokenIdHash = _securityService.GetSha256Hash(refreshTokenSerial);
        return await _context.UserTokens.AsQueryable()
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash);
    }
}