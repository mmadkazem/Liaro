namespace Liaro.Application.ExternalServices.Security;


public interface ISecurityService
{
    string GetSha256Hash(string input);
    Guid CreateCryptographicallySecureGuid();
    string GetRefreshTokenSerial(string refreshTokenValue);
}