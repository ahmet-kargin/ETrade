using System.ComponentModel.DataAnnotations;

namespace ETrade.WebUI.Models.Login;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }

    public string Address { get; set; }
}
