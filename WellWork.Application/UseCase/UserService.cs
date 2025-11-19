using WellWork.Domain;
using WellWork.Domain.Interfaces;

namespace WellWork.Application;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User> CreateUserAsync(string username, string passwordHash, string role)
    {
        var user = new User(Guid.NewGuid(), username, passwordHash);
        await _userRepo.AddAsync(user);
        return user;
    }

    public Task<User?> GetByIdAsync(Guid id)
        => _userRepo.GetByIdAsync(id);

    public Task<User?> GetByUsernameAsync(string username)
        => _userRepo.GetByUsernameAsync(username);

    public async Task UpdatePasswordAsync(Guid userId, string newPasswordHash)
    {
        var user = await _userRepo.GetByIdAsync(userId)
                   ?? throw new Exception("Usuário não encontrado");

        user.UpdatePassword(newPasswordHash);
        await _userRepo.UpdateAsync(user);
    }
    

    public async Task<(IEnumerable<User> Items, long TotalCount)> GetPagedAsync(int page, int pageSize)
        => await _userRepo.ListAsync(page, pageSize);
    
    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id);

        if (user == null)
            throw new Exception("Usuário não encontrado.");

        await _userRepo.DeleteAsync(user);
    }
}