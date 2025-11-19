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
        await _context.SaveChangesAsync();
    }

    public async  Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<User> Items, long TotalCount)> ListAsync(int pageIndex, int pageSize)
    {
        var query = _context.Users.AsQueryable();

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(u => u.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }
}