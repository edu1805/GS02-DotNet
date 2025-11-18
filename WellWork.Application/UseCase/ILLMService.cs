namespace WellWork.Application;

public record LLMResult(string Message, decimal Confidence);

public interface ILLMService
{
    Task<LLMResult> GenerateMessageForCheckInAsync(string mood, string energy, string? notes);
}