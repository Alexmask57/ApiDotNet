using Microsoft.AspNetCore.Identity;

namespace ApiDotNet.Models.Authentication;

public class ApplicationUser : IdentityUser<string>
{
    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    public virtual ICollection<Credentials> Credentials { get; set; }

    public ApplicationUser()
    {
    }

    public ApplicationUser(string username) : base(username)
    {
    }
    
}