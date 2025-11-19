using WellWork.Domain;
using WellWork.Domain.Enums;
using WellWork.Domain.Interfaces;

namespace WellWork.Application;

public class CheckInService : ICheckInService
{
    private readonly ICheckInRepository _checkInRepo;
    private readonly IUserRepository _userRepo;
    private readonly IGeneratedMessageService _messageService;
    private readonly ILLMService _llmService;

    public CheckInService(
        ICheckInRepository checkInRepo,
        IUserRepository userRepo,
        IGeneratedMessageService messageService,
        ILLMService llmService)
    {
        _checkInRepo = checkInRepo;
        _userRepo = userRepo;
        _messageService = messageService;
        _llmService = llmService;
    }

    public async Task<CheckIn> CreateCheckInAsync(Guid userId, Mood mood, EnergyLevel energy, string? notes)
    {
        var user = await _userRepo.GetByIdAsync(userId)
            ?? throw new Exception("Usuário não encontrado");

        var checkIn = new CheckIn(Guid.NewGuid(), userId, mood, energy, notes);
        await _checkInRepo.AddAsync(checkIn);

        return checkIn;
    }

    public Task<CheckIn?> GetByIdAsync(Guid id)
        => _checkInRepo.GetByIdAsync(id);

    public Task<(IEnumerable<CheckIn> Items, long TotalCount)> GetByUserIdAsync(Guid userId, int page, int pageSize)
        => _checkInRepo.ListByUserAsync(userId, page, pageSize);

    public async Task UpdateMoodAsync(Guid id, Mood newMood)
    {
        var checkIn = await _checkInRepo.GetByIdAsync(id)
            ?? throw new Exception("Check-in não encontrado");

        checkIn.UpdateMood(newMood);
        await _checkInRepo.UpdateAsync(checkIn);
    }

    public async Task UpdateEnergyAsync(Guid id, EnergyLevel newEnergy)
    {
        var checkIn = await _checkInRepo.GetByIdAsync(id)
            ?? throw new Exception("Check-in não encontrado");

        checkIn.UpdateEnergy(newEnergy);
        await _checkInRepo.UpdateAsync(checkIn);
    }

    public async Task UpdateNotesAsync(Guid id, string? notes)
    {
        var checkIn = await _checkInRepo.GetByIdAsync(id)
            ?? throw new Exception("Check-in não encontrado");

        checkIn.UpdateNotes(notes);
        await _checkInRepo.UpdateAsync(checkIn);
    }

    public async Task<GeneratedMessage> GenerateLLMMessageAsync(Guid checkInId)
    {
        var checkIn = await _checkInRepo.GetByIdAsync(checkInId)
            ?? throw new Exception("Check-in não encontrado");

        var llmResult = await _llmService.GenerateMessageForCheckInAsync(
            checkIn.Mood.ToString(),
            checkIn.Energy.ToString(),
            checkIn.Notes
        );

        var msg = await _messageService.CreateAsync(checkIn.Id, llmResult.Message, llmResult.Confidence);

        checkIn.AddOrUpdateGeneratedMessage(msg);
        await _checkInRepo.UpdateAsync(checkIn);

        return msg;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var checkIn = await _checkInRepo.GetByIdAsync(id);
        if (checkIn == null)
            throw new Exception("CheckIn não encontrado.");

        await _checkInRepo.DeleteAsync(checkIn);
    }
}