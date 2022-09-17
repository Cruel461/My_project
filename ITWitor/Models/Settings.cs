using ITWitor.Models;
using ITWitor.Models;

using Microsoft.EntityFrameworkCore;

namespace ITWitor.Data
{
  public class Settings : BaseModel
  {
    public bool? IsHandOfTheDeadActive { get; set; }
    public Organization? Organization { get; set; }
    public PortalSettings? PortalSettings { get; set; }

    private static Settings? instance;

    public static Settings Inizialize()
    {
      instance ??= new Settings();
      using (var context = new ApplicationDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<ApplicationDbContext>()))
      {
        instance = context?.Settings?
            .Include(s => s.Organization)
            .ThenInclude(b => b.Contacts)
            .ThenInclude(bc => bc.Address)
            .Include(s => s.Organization)
            .ThenInclude(o => o.Contacts)
            .ThenInclude(mc => mc.Address)
            .Include(s => s.Organization)
            .ThenInclude(o => o.Logo)
            .Include(s => s.PortalSettings).ThenInclude(ps => ps!.Themes)
            .FirstOrDefault();
        if (instance == null)
        {
          instance = new Settings { IsHandOfTheDeadActive = false };
          context?.Settings.Add(instance);
          context?.SaveChanges();
        }
        if (instance.PortalSettings == null)
        {
          instance.PortalSettings = new PortalSettings();
          context?.Update(instance);
          context?.SaveChanges();
        }
        if (instance.Organization == null)
        {
          instance.Organization = new Organization();
          context?.Update(instance);
          context?.SaveChanges();
        }
      }
      return instance;
    }
  }
}
