using System.ComponentModel.DataAnnotations;

namespace ApiDotNet.Models.Dto;

public class ChangePasswordRequest
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
    
    [Required]
    public string NewPassword { get; set; } = null!;
    
    [Required]
    [Compare("NewPassword")]
    public string NewPasswordConfirmed { get; set; } = null!;
    
}