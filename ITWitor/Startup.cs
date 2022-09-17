using ITWitor.Data;
using ITWitor.Models;
using ITWitor.Services.Chat;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITWitor
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;
    }

    public static IConfiguration? Configuration { get; internal set; }
    public static IWebHostEnvironment? Environment { get; internal set; }

    public void ConfigureServices(IServiceCollection services)
    {
      string connectionString = Environment.IsDevelopment() ? Configuration.GetConnectionString("Default") : Configuration.GetConnectionString("Default");

      services.AddDbContext<ApplicationDbContext>(options =>

       options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 23))));

      services.AddDatabaseDeveloperPageExceptionFilter();

      services.AddIdentity<AppUser, IdentityRole>(options =>
      {
        options.User.AllowedUserNameCharacters = "абвгдеёжзийклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
      })
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders();

      services.ConfigureApplicationCookie(options =>
      {
        options.LoginPath = "/";
        options.AccessDeniedPath = "/";
        options.SlidingExpiration = true;
      });

      services.AddSignalR((hubOptions) =>
      {
        hubOptions.KeepAliveInterval = TimeSpan.FromMilliseconds(15000);
      });

      services.AddControllersWithViews();

      services.AddScoped<ApplicationDbContext>();

      services.AddScoped<Settings>((e) => Settings.Inizialize());

      services.AddSession();
      services.AddControllersWithViews();
      services.AddRazorPages();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      Environment = env;
      if (env.IsDevelopment())
      {
        app.UseMigrationsEndPoint();
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
             name: "default",
             pattern: "{action}",
             defaults: new { controller = "Home", action = "Index" });

        endpoints.MapControllerRoute(
           name: "admin",
           pattern: "{controller}/{action}");

        endpoints.MapControllerRoute(
            name: "store",
            pattern: "{controller}/{action}/{categoryName}/{productName?}/{article?}",
            defaults: new { controller = "Store", action = "Catalog" });

        endpoints.MapHub<ChatHub>("/chat");

        endpoints.MapRazorPages();
      });
    }
  }
}
