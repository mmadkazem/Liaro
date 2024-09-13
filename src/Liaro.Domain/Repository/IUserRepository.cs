namespace Liaro.Domain.Repository;


public interface IUserRepository
{
    void Add(User user);
    void Remove(User user);
    void Update(User user);
    Task<User> FindAsync(int userId, CancellationToken token);
    Task<User> FindAsyncByMobileAsync(string mobile, CancellationToken token);
    Task<bool> AnyAsyncUserNameAsync(string userName, CancellationToken token);
    Task<List<Role>> FindUserRolesAsync(int userId, CancellationToken token);
    Task<User> FindUserAsync(string username, string password, CancellationToken token);
    Task<User> FindByMobileAndLoginCodeAsync(string mobile, string code, CancellationToken token);
}