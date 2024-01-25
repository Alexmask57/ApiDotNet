using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiDotNet.Models.Authentication;
using Microsoft.EntityFrameworkCore;

namespace ApiDotNet.Models;

[PrimaryKey(nameof(Id), nameof(Description))]
public class Credentials
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
    public Guid Id { get; set; }
    public string Description { get; set; }
    public ApplicationUser User { get; set; }
    public string? Credential1 { get; set; }
    public string? Credential2 { get; set; }
    public string? Credential3 { get; set; }
}