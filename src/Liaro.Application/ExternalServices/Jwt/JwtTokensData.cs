namespace Liaro.Application.ExternalServices.Jwt;

public class JwtTokensData
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required string RefreshTokenSerial { get; set; }
    public required IEnumerable<Claim> Claims { get; set; }
}