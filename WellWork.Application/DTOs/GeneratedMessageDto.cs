namespace WellWork.Application.DTOs;

public class GeneratedMessageDto
{
    public Guid Id { get; set; }
    public Guid CheckInId { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public DateTimeOffset GeneratedAt { get; set; }
}