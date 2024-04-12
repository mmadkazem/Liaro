namespace Liaro.Infrastructure.ExternalServices.Jwt;


public class TokenFactoryService : ITokenFactoryService
{
    private readonly ISecurityService _securityService;
    private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
    private readonly IEntityBaseRepository<UserRole> _roleRepository;
    private readonly ILogger<TokenFactoryService> _logger;

    public TokenFactoryService(
        ISecurityService securityService,
        IEntityBaseRepository<UserRole> roleRepository,
        IOptionsSnapshot<BearerTokensOptions> configuration,
        ILogger<TokenFactoryService> logger)
    {
        _securityService = securityService;
        _securityService.CheckArgumentIsNull(nameof(_securityService));

        _roleRepository = roleRepository;
        _roleRepository.CheckArgumentIsNull(nameof(roleRepository));

        _configuration = configuration;
        _configuration.CheckArgumentIsNull(nameof(configuration));

        _logger = logger;
        _logger.CheckArgumentIsNull(nameof(logger));
    }


    public async Task<JwtTokensData> CreateJwtTokensAsync(User user)
    {
        var (accessToken, claims) = await createAccessTokenAsync(user);
        var (refreshTokenValue, refreshTokenSerial) = createRefreshToken();
        return new JwtTokensData
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            RefreshTokenSerial = refreshTokenSerial,
            Claims = claims
        };
    }

    private (string RefreshTokenValue, string RefreshTokenSerial) createRefreshToken()
    {
        var refreshTokenSerial = _securityService.CreateCryptographicallySecureGuid().ToString().Replace("-", "");

        var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, _securityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issuer
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer, ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issued at
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Value.Issuer),
                // for invalidation
                new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String, _configuration.Value.Issuer)
            };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _configuration.Value.Issuer,
            audience: _configuration.Value.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
            signingCredentials: creds);
        var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return (refreshTokenValue, refreshTokenSerial);
    }

    public string GetRefreshTokenSerial(string refreshTokenValue)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenValue))
        {
            return null;
        }

        ClaimsPrincipal decodedRefreshTokenPrincipal = null;
        try
        {
            decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                refreshTokenValue,
                new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key)),
                    ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                    ValidateLifetime = true, // validate the expiration
                    ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                },
                out _
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to validate refreshTokenValue: `{refreshTokenValue}`.");
        }

        return decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
    }

    private async Task<(string AccessToken, IEnumerable<Claim> Claims)> createAccessTokenAsync(User user)
    {
        var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, _securityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issuer
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer, ClaimValueTypes.String, _configuration.Value.Issuer),
                // Issued at
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Value.Issuer),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim("DisplayName", user.DisplayName, ClaimValueTypes.String, _configuration.Value.Issuer),
                // to invalidate the cookie
                new Claim(ClaimTypes.SerialNumber, user.SerialNumber, ClaimValueTypes.String, _configuration.Value.Issuer),
                // custom data
                new Claim(ClaimTypes.UserData, user.Id.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer)
            };

        // add roles
        var roles = await _roleRepository.GetAllQueryable()
                            .Include(x => x.Role)
                            .AsNoTracking()
                            .Where(x => x.UserId == user.Id)
                            .Select(x => x.Role)
                            .OrderBy(x => x.Name)
                            .ToListAsync();

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name, ClaimValueTypes.String, _configuration.Value.Issuer));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: _configuration.Value.Issuer,
            audience: _configuration.Value.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes),
            signingCredentials: creds);
        return (new JwtSecurityTokenHandler().WriteToken(token), claims);
    }
}
