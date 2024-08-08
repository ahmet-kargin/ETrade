using System.ComponentModel.DataAnnotations;

namespace ETrade.Domain.Entities;

public class LoginHistory
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
    public LoginHistoryType Type { get; set; }
}

public enum LoginHistoryType
{
    Login = 1,
    Logout = 2
}
