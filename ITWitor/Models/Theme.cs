using ITWitor.Data;

namespace ITWitor.Models
{
  public class Theme : ITWitor.Models.BaseModel
  {
    [Newtonsoft.Json.JsonProperty("cssPath")]
    [System.ComponentModel.DisplayName("Путь к .css")]
    public string CssPath { get; set; }

    public bool IsSelected { get; set; }
  }
}
