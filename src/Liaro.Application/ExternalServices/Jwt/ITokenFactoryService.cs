namespace Liaro.Application.ExternalServices.Jwt;


public interface ITokenFactoryService
{
    Task<JwtTokensData> CreateJwtTokensAsync(User? user);
}
