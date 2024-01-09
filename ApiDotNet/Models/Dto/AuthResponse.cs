namespace ApiDotNet.Models.Dto;

public class AuthResponse
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime TokenExpirationDate { get; set; }
}