using WellWork.Domain.Enums;

namespace WellWork.Application.DTOs;

public record CheckInResponseDto(
    Guid Id,
    Guid UserId,
    Mood Mood,
    EnergyLevel Energy,
    string? Notes,
    DateTimeOffset CreatedAt,
    GeneratedMessageResponseDto? GeneratedMessage
);
