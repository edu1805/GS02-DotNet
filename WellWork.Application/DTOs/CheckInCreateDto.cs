using WellWork.Domain.Enums;

namespace WellWork.Application.DTOs;

public record CheckInCreateDto(
    Guid UserId,
    Mood Mood,
    EnergyLevel Energy,
    string? Notes
);
