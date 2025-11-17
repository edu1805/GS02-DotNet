namespace WellWork.Domain;

public class GeneratedMessage
{
    public Guid Id { get; private set; }
    public Guid CheckInId { get; private set; }
    public string Message { get; private set; }
    public decimal Confidence { get; private set; }
    public DateTimeOffset GeneratedAt { get; private set; }

    public CheckIn? CheckIn { get; private set; }
    
    protected GeneratedMessage() { }

    public GeneratedMessage(Guid id, Guid checkInId, string message, decimal confidence)
    {
        Id = id;

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Mensagem obrigatória");

        if (confidence < 0 || confidence > 1)
            throw new ArgumentOutOfRangeException(nameof(confidence));

        CheckInId = checkInId;
        Message = message;
        Confidence = confidence;
        GeneratedAt = DateTimeOffset.UtcNow;
    }
}