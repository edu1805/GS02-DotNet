using WellWork.Domain;

namespace WellWork.Application;

public interface IGeneratedMessageService
{
    Task<GeneratedMessage> CreateAsync(Guid checkInId, string message, decimal confidence);
    Task<GeneratedMessage?> GetByCheckInIdAsync(Guid checkInId);
}