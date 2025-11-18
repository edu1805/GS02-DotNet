using WellWork.Domain;
using WellWork.Domain.Enums;

namespace WellWork.Application;

public interface ICheckInService
{
    Task<CheckIn> CreateCheckInAsync(Guid userId, Mood mood, EnergyLevel energy, string? notes);

    Task<CheckIn?> GetByIdAsync(Guid id);
    Task<(IEnumerable<CheckIn> Items, long TotalCount)> GetByUserIdAsync(Guid userId, int page, int pageSize);

    Task UpdateMoodAsync(Guid checkInId, Mood newMood);
    Task UpdateEnergyAsync(Guid checkInId, EnergyLevel newEnergy);
    Task UpdateNotesAsync(Guid checkInId, string? newNotes);

    /// <summary>
    /// Gera mensagem via LLM e salva GeneratedMessage associada
    /// </summary>
    Task<GeneratedMessage> GenerateLLMMessageAsync(Guid checkInId);
}