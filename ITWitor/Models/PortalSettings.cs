
using ITWitor.Models;

using ITWitor.Models;

using System.ComponentModel.DataAnnotations.Schema;

namespace ITWitor.Data
{
  public class PortalSettings : BaseModel
  {
    [Newtonsoft.Json.JsonIgnore]
    public Settings Settings { get; set; }

    [ForeignKey("Settings")]
    public int SettingsId { get; set; }

    [Newtonsoft.Json.JsonProperty("isOnline")]
    [System.ComponentModel.DisplayName("Состояние")]
    public bool IsOnline { get; set; }



    [Newtonsoft.Json.JsonProperty("showNavigation")]
    [System.ComponentModel.DisplayName("Навигация")]
    public bool ShowNavigation { get; set; }

    [Newtonsoft.Json.JsonProperty("themes", DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
    public List<Theme> Themes { get; set; } = new List<Theme>();
  }
}
