namespace Liaro.Infrastructure.ExternalServices.Jwt;


public class TokenFactoryService : ITokenFactoryService
{
    private readonly ISecurityService _securityService;
    private readonly IOptions<BearerTokensOptions> _configuration;
    private readonly IUnitOfWork _uow;

    public TokenFactoryService(
        ISecurityService securityService,
        IOptions<BearerTokensOptions> configuration,
        IUnitOfWork uow)
    {
        _securityService = securityService;
        _securityService.CheckArgumentIsNull(nameof(securityService));

        _configuration = configuration;
        _configuration.CheckArgumentIsNull(nameof(configuration));

        _uow = uow;
        _uow.CheckArgumentIsNull(nameof(uow));
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
        var roles = await _uow.Users.FindUserRolesAsync(user.Id);

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
