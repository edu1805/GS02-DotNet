namespace WellWork.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);

    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);

    Task<(IEnumerable<User> Items, long TotalCount)>
        ListAsync(int pageIndex, int pageSize);
}