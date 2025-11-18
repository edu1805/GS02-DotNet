namespace WellWork.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    private readonly List<CheckIn> _checkIns = new();
    public IReadOnlyCollection<CheckIn> CheckIns => _checkIns.AsReadOnly();
    
    protected User() { }
    
    public User(Guid id, string username, string passwordHash)
    {
        Id = id;
        Username = username ?? throw new ArgumentException("username é obrigatório");
        PasswordHash = passwordHash ?? throw new ArgumentException("passwordHash é obrigatório");
    }
    
    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Senha inválida");

        PasswordHash = newPasswordHash;
    }
    

    internal void AddCheckIn(CheckIn checkIn)
    {
        if (checkIn == null) throw new ArgumentNullException(nameof(checkIn));
        _checkIns.Add(checkIn);
    }
}