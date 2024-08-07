﻿namespace ETrade.Domain.Entities;

public class User //: IdentityUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }
    public string PasswordHash { get; set; }
}
