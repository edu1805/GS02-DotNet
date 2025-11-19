using Microsoft.EntityFrameworkCore;
using WellWork.Domain;
using WellWork.Domain.Interfaces;

namespace WellWork.Infrastructure.Persistence;

public class GeneratedMessageRepository : IGeneratedMessageRepository
{
    private readonly WellWorkDbContext _context;

    public GeneratedMessageRepository(WellWorkDbContext context)
    {
        _context = context;
    }

    public async Task<GeneratedMessage?> GetByIdAsync(Guid id)
    {
        return await _context.GeneratedMessages
            .Include(m => m.CheckIn)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<GeneratedMessage?> GetByCheckInIdAsync(Guid checkInId)
    {
        return await _context.GeneratedMessages
            .Include(m => m.CheckIn)
            .FirstOrDefaultAsync(m => m.CheckInId == checkInId);
    }

    public async Task AddAsync(GeneratedMessage message)
    {
        await _context.GeneratedMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(GeneratedMessage message)
    {
        _context.GeneratedMessages.Update(message);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(GeneratedMessage message)
    {
        _context.GeneratedMessages.Remove(message);
        return Task.CompletedTask;
    }
}