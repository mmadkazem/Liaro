namespace Liaro.Application.ExternalServices.Jwt;


public record JwtTokensResponse(string AccessToken, string RefreshToken);