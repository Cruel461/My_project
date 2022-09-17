using ITWitor.Models;

using Newtonsoft.Json;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITWitor.Models
{
  public class MainContacts : Contacts
    {
        [JsonIgnore]
        public Organization? Organization { get; set; }

        [ForeignKey("Organization")]
        public int? ParentId { get; set; }

    }

    public class Contacts : BaseModel
    {
        [DisplayName("Основной телефон")]
        [JsonProperty("mainPhone", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? MainPhone { get; set; }

        [DisplayName("Все телефоны")]
        [JsonProperty("phones", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Phones { get; set; }

        [DisplayName("Email")]
        [JsonProperty("email", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Email { get; set; }

        [DisplayName("Ссылка на страницу")]
        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Url { get; set; }

        [DisplayName("Адрес")]
        [JsonProperty("address", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Address? Address { get; set; }
    }
}
