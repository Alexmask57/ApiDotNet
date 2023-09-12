using Microsoft.AspNetCore.Identity;

namespace ApiDotNet.Models.Authentication;

public class ApplicationRole : IdentityRole<string>
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    public virtual ICollection<IdentityRoleClaim<string>> RoleClaims { get; set; }

    public ApplicationRole(string role) : base(role)
    {
        NormalizedName = role.ToUpper();
    }

    public ApplicationRole()
    {
        
    }
}