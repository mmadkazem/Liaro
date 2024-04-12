namespace Liaro.Domain.Repository;


public interface IUserRepository
{
    void Add(User user);
    void Remove(User user);
    void Update(User user);
    Task<User> FindAsync(int userId);
    Task<User> FindAsyncByMobile(string mobile);
    Task<bool> AnyAsyncUserName(string userName);
    Task<List<Role>> FindUserRolesAsync(int userId);
    Task<User> FindUserAsync(string username, string password);
    Task<User> FindByMobileAndLoginCode(string mobile, string code);
}