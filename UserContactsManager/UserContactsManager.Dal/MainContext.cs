using Microsoft.EntityFrameworkCore;
using UserContactsManager.Dal.Configurations;
using UserContactsManager.Dal.Entities;

namespace UserContactsManager.Dal;

public class MainContext : DbContext
{
    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new ContactConfigurations());
        modelBuilder.ApplyConfiguration(new UserRoleConfigurations());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfigurations());





    }

}
