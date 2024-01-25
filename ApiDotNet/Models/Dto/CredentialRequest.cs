using System.ComponentModel.DataAnnotations;

namespace ApiDotNet.Models.Dto;

public class CredentialRequest
{
    [Required]
    public string Description { get; set; }
    public string Credential1 { get; set; }
    public string Credential2 { get; set; }
    public string Credential3 { get; set; }
}