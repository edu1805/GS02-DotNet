using Microsoft.EntityFrameworkCore;
using WellWork.Domain;
using WellWork.Domain.Interfaces;

namespace WellWork.Infrastructure.Persistence;

public class CheckInRepository : ICheckInRepository
{
    private readonly WellWorkDbContext _context;

    public CheckInRepository(WellWorkDbContext context)
    {
        _context = context;
    }

    public async Task<CheckIn?> GetByIdAsync(Guid id)
    {
        return await _context.CheckIns
            .Include(c => c.User)
            .Include(c => c.GeneratedMessage)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(CheckIn checkIn)
    {
        await _context.CheckIns.AddAsync(checkIn);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CheckIn checkIn)
    {
        _context.CheckIns.Update(checkIn);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CheckIn checkIn)
    {
        _context.CheckIns.Remove(checkIn);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<CheckIn> Items, long TotalCount)>
        ListByUserAsync(Guid userId, int pageIndex, int pageSize)
    {
        var query = _context.CheckIns
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt);

        var total = await query.CountAsync();

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.GeneratedMessage)
            .ToListAsync();

        return (items, total);
    }
}