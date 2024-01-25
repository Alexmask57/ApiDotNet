using ApiDotNet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ApiDotNet.Models.Authentication;

namespace ApiDotNet.Context;

public class UsersContext : IdentityDbContext<
    ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public UsersContext (DbContextOptions<UsersContext> options) : base(options)
    {
        
    }
    
    public DbSet<Credentials>? Credentials { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ApplicationUser>(b =>
        {
            // TODO Ajout des delete cascade
            
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            
            // Ajout de credentials pour chaques users
            b.HasMany(e => e.Credentials)
                .WithOne(e => e.User);
            
            // Ajout de l'id auto-incrémenté
            b.Property(p => p.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ApplicationRole>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            b.HasMany(e => e.RoleClaims)
                .WithOne()
                .HasForeignKey(e => e.RoleId)
                .IsRequired();
            
            // Ajout de l'id auto-incrémenté
            b.Property(p => p.Id).ValueGeneratedOnAdd();
        });

        List<ApplicationRole> applicationRoles = new List<ApplicationRole>()
        {
            new ApplicationRole("Default") { Id = Guid.NewGuid().ToString() },
            new ApplicationRole("Developer") { Id = Guid.NewGuid().ToString() },
            new ApplicationRole("Admin") { Id = Guid.NewGuid().ToString() }
        };
        ApplicationRole adminRole = applicationRoles.Find(x => x.Name == "Admin");
        List<ApplicationUser> applicationUsers = new List<ApplicationUser>()
        {
            // TODO Ajouter des utilisateurs par défaut
            // Problème : password non haché
            // new ApplicationUser("Alexmask")
            // {
            //     Id = Guid.NewGuid().ToString(),
            //     Email = "alexismaskio@gmail.com",
            //     PasswordHash = "admin"
            // },
        };
        List<ApplicationUserRole> applicationUserRoles = new List<ApplicationUserRole>();
        
        // Ajoute le rôle admin
        applicationUsers.ForEach(delegate(ApplicationUser user)
        {
            applicationUserRoles.Add(new ApplicationUserRole()
            {
                RoleId = adminRole.Id, UserId = user.Id
            });
        });
        
        modelBuilder.Entity<ApplicationRole>().HasData(applicationRoles);
        
        modelBuilder.Entity<ApplicationUser>().HasData(applicationUsers);

        modelBuilder.Entity<ApplicationUserRole>().HasData(applicationUserRoles);
    }

}