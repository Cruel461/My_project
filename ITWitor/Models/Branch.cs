using ITWitor.Models;

using Newtonsoft.Json;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace ITWitor.Models
{
  public class Branch : BaseModel
    {
        [JsonIgnore]
        public Organization? Organization { get; set; }

        [ForeignKey("Organization")]
        public int? ParentId { get; set; }

        [DisplayName("Описание")]
        [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        [DisplayName("Контакты")]
        [JsonProperty("contacts", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BranchContacts? Contacts { get; set; }
    }
}
