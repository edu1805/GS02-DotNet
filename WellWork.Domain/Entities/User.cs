namespace WellWork.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    private readonly List<CheckIn> _checkIns = new();
    public IReadOnlyCollection<CheckIn> CheckIns => _checkIns.AsReadOnly();
    
    protected User() { }
    
    public User(Guid id, string username, string passwordHash, string role = "ROLE_USER")
    {
        Id = id;
        Username = username ?? throw new ArgumentException("username é obrigatório");
        PasswordHash = passwordHash ?? throw new ArgumentException("passwordHash é obrigatório");
        Role = role;
    }
    
    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Senha inválida");

        PasswordHash = newPasswordHash;
    }

    public void UpdateRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role inválida");

        Role = role;
    }

    internal void AddCheckIn(CheckIn checkIn)
    {
        if (checkIn == null) throw new ArgumentNullException(nameof(checkIn));
        _checkIns.Add(checkIn);
    }
}