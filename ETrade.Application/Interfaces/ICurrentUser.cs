namespace ETrade.Application.Interfaces;

public interface ICurrentUser
{
    int Id { get;}
    string FirstName { get;}
    string LastName { get;}
    string Email { get;}
    string Phone { get;}
    string Address { get; }
}
