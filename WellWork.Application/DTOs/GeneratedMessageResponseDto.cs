namespace WellWork.Application.DTOs;

public record GeneratedMessageResponseDto(
    Guid Id,
    string Message,
    decimal Confidence,
    DateTimeOffset GeneratedAt
);
