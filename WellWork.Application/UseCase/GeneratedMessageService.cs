using WellWork.Domain;
using WellWork.Domain.Interfaces;

namespace WellWork.Application;

public class GeneratedMessageService : IGeneratedMessageService
{
    private readonly IGeneratedMessageRepository _messageRepo;

    public GeneratedMessageService(IGeneratedMessageRepository messageRepo)
    {
        _messageRepo = messageRepo;
    }

    public async Task<GeneratedMessage> CreateAsync(Guid checkInId, string message, decimal confidence)
    {
        var msg = new GeneratedMessage(Guid.NewGuid(), checkInId, message, confidence);
        await _messageRepo.AddAsync(msg);
        return msg;
    }

    public Task<GeneratedMessage?> GetByCheckInIdAsync(Guid checkInId)
        => _messageRepo.GetByCheckInIdAsync(checkInId);
}