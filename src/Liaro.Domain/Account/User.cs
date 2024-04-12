namespace Liaro.Domain.Account;

public class User : IEntity
{
    public User()
    {
        UserRoles = new HashSet<UserRole>();
        UserTokens = new HashSet<UserToken>();
    }

    public int Id { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string DisplayName { get; set; }

    public bool IsActive { get; set; }

    public required string Email { get; set; }

    public string? Mobile { get; set; }

    public string? LoginCode { get; set; }

    public DateTimeOffset? MobileLoginExpire { get; set; }

    public DateTimeOffset? LastLoggedIn { get; set; }

    /// <summary>
    /// every time the user changes his Password,
    /// or an admin changes his Roles or stat/IsActive,
    /// create a new `SerialNumber` GUID and store it in the DB.
    /// </summary>
    public string? SerialNumber { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

    public virtual ICollection<UserToken> UserTokens { get; set; } = new HashSet<UserToken>();
}
