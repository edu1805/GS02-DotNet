using WellWork.Domain;

namespace WellWork.Application;

public interface IUserService
{
    Task<User> CreateUserAsync(string username, string passwordHash, string role);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    
    Task UpdatePasswordAsync(Guid userId, string newPasswordHash);

    Task<(IEnumerable<User> Items, long TotalCount)> GetPagedAsync(int page, int pageSize);
    Task DeleteUserAsync(Guid id);

}