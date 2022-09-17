using ITWitor.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ITWitor.Data
{
  public class ApplicationDbContext : IdentityDbContext<AppUser>
  {
    public DbSet<Visit> Visits { get; set; }
    public DbSet<Settings> Settings { get; set; }
    public DbSet<StaticFile> Files { get; set; }
    public DbSet<Page> Pages { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
    {
      try
      {
        if (this.Users.FirstOrDefault(u => u.Email == "pycek@list.ru") == null) CreateSU();
      }
      catch { }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySql(Startup.Environment.IsDevelopment() ? Startup.Configuration.GetConnectionString("Default") : Startup.Configuration.GetConnectionString("Default"), new MySqlServerVersion(new Version(8, 0, 23)));
      base.OnConfiguring(optionsBuilder);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

      //modelBuilder.Entity<Page>().HasMany<Page>().WithOne(p => p.Parent).OnDelete(DeleteBehavior.SetNull);

      //modelBuilder.Entity<Page>()
      //     .HasMany<Page>(s => s.Childrens)
      //     .WithOne(ad => ad.Parent)
      //     .HasForeignKey(ad => ad.ParentId);

      //.WillCascadeOnDelete();
      //modelBuilder.Entity<Page>().HasOne<Page>(d => d.Parent).WithOne(p => p.Parent).HasForeignKey(k => k.);
      base.OnModelCreating(modelBuilder);
    }




    public void CreateSU()
    {
      AppUser pycek = new AppUser();
      pycek.Name = "Руслан";
      pycek.Email = "pycek@list.ru";
      pycek.UserName = pycek.Email;
      PasswordHasher<string> hasher = new PasswordHasher<string>();
      pycek.PasswordHash = hasher.HashPassword(pycek.UserName, "3339393");
      pycek.EmailConfirmed = true;
      pycek.NormalizedEmail = pycek.Email.ToUpper();
      pycek.NormalizedUserName = pycek.NormalizedEmail;

      IdentityRole guestRole = new IdentityRole("guest");
      IdentityRole adminRole = new IdentityRole("admin");
      IdentityRole managerRole = new IdentityRole("manager");
      IdentityRole clientRole = new IdentityRole("client");
      Roles.Add(guestRole);
      Roles.Add(clientRole);
      Roles.Add(managerRole);
      Roles.Add(adminRole);

      Users.Add(pycek);

      IdentityUserRole<string> adminUserRole = new IdentityUserRole<string> { RoleId = adminRole.Id, UserId = pycek.Id };

      UserRoles.Add(adminUserRole);

      SaveChanges();
    }

    public void CreateUser()
    {
      AppUser lucie = new AppUser();
      lucie.Name = "Lucie";
      lucie.Email = "gipnosik1@gmail.com";
      lucie.UserName = lucie.Email;
      PasswordHasher<string> hasher = new PasswordHasher<string>();
      lucie.PasswordHash = hasher.HashPassword(lucie.UserName, "gipnosik1");
      lucie.EmailConfirmed = true;
      lucie.NormalizedEmail = lucie.Email.ToUpper();
      lucie.NormalizedUserName = lucie.NormalizedEmail;

      IdentityRole managerRole = new IdentityRole("manager");



      Users.Add(lucie);

      var role = Roles.FirstOrDefault(r => r.Name == "manager");

      IdentityUserRole<string> adminUserRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = lucie.Id };

      UserRoles.Add(adminUserRole);

      SaveChanges();
    }

    public DbSet<ITWitor.Models.Organization>? Organization { get; set; }
  }

  public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
      optionsBuilder.UseMySql("server=37.77.105.24;database=develop;uid=pycek;password=6m7sd38L;ConvertZeroDateTime=True", new MySqlServerVersion(new Version(8, 0, 23)));
      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }

  //public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  //{
  //    public ApplicationDbContext CreateDbContext(string[] args)
  //    {
  //        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
  //        optionsBuilder.UseMySql(Startup.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 23)));
  //        return new ApplicationDbContext(optionsBuilder.Options);
  //    }
  //}
}