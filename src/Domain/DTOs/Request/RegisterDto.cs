using System.ComponentModel.DataAnnotations;

namespace NotesAPI.DTOs.Request;

public class RegisterUserDto
{
    [Required(ErrorMessage = "Phone is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}