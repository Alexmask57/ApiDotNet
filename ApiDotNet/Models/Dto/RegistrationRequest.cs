using System.ComponentModel.DataAnnotations;

namespace ApiDotNet.Models.Dto;

public class RegistrationRequest
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}