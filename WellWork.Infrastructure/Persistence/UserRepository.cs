using Microsoft.EntityFrameworkCore;
using WellWork.Domain;
using WellWork.Domain.Interfaces;

namespace WellWork.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly WellWorkDbContext _context;

    public UserRepository(WellWorkDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task<(IEnumerable<User> Items, long TotalCount)> ListAsync(int pageIndex, int pageSize)
    {
        var query = _context.Users.AsQueryable();

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(u => u.Id)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}