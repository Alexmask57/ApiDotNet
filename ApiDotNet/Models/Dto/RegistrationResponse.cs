namespace ApiDotNet.Models.Dto;

public class RegistrationResponse
{
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
}