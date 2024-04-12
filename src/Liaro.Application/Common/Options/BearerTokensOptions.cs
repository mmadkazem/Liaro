namespace Liaro.Application.Common.Options;

public class BearerTokensOptions
{
    public string? Key { set; get; }
    public string? Issuer { set; get; }
    public string? Audience { set; get; }
    public int AccessTokenExpirationMinutes { set; get; }
    public int RefreshTokenExpirationMinutes { set; get; }
    public bool AllowMultipleLoginsFromTheSameUser { set; get; }
    public bool AllowSignOutAllUserActiveClients { set; get; }
}