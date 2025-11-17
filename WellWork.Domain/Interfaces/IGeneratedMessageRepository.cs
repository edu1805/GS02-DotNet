namespace WellWork.Domain.Interfaces;

public interface IGeneratedMessageRepository
{
    Task<GeneratedMessage?> GetByIdAsync(Guid id);
    Task<GeneratedMessage?> GetByCheckInIdAsync(Guid checkInId);

    Task AddAsync(GeneratedMessage message);
    Task UpdateAsync(GeneratedMessage message);
    Task DeleteAsync(GeneratedMessage message);
}