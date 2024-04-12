namespace Liaro.Domain.Account;

public class Role : IEntity
{
    public Role()
    {
        UserRoles = new HashSet<UserRole>();
    }

    public int Id { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}