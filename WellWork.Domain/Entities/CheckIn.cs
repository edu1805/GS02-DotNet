using WellWork.Domain.Enums;

namespace WellWork.Domain;

public class CheckIn
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Mood Mood { get; private set; }
    public EnergyLevel Energy { get; private set; }
    public string? Notes { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public GeneratedMessage? GeneratedMessage { get; private set; }
    
    protected CheckIn() { }
    
    public CheckIn(Guid id, Guid userId, Mood mood, EnergyLevel energy, string? notes = null)
    {
        Id = id;
        UserId = userId;
        Mood = mood;
        Energy = energy;
        Notes = notes;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public void AddOrUpdateGeneratedMessage(GeneratedMessage msg)
    {
        if (msg == null) throw new ArgumentNullException();

        if (msg.CheckInId != Id && msg.CheckInId != null)
            throw new InvalidOperationException("Mensagem de outro check-in.");

        GeneratedMessage = msg;
    }

    public void UpdateMood(Mood mood) => Mood = mood;
    public void UpdateEnergy(EnergyLevel energy) => Energy = energy;
    public void UpdateNotes(string? notes) => Notes = notes;
}