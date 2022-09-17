using ITWitor.Data;
using ITWitor.Models;

using Newtonsoft.Json;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITWitor.Models
{
  public class Organization : BaseModel
  {

    [ForeignKey("Parent")]
    public int? ParentId { get; set; }
    [JsonIgnore]
    public Settings? Parent { get; set; }


    [DisplayName("Полное наименование")]
    [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public override string? Name { get; set; }

    [DisplayName("Краткое наименование")]
    [JsonProperty("shortName", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? ShortName { get; set; }

    [DisplayName("Описание")]
    [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Description { get; set; }

    [DisplayName("Контакты")]
    [JsonProperty("contacts", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Contacts? Contacts { get; set; }

    [DisplayName("Логотип")]
    [ForeignKey("Logo")]
    public int? LogoId { get; set; }
  
    [DisplayName("Логотип")]
    [JsonProperty("logo", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public StaticFile? Logo { get; set; }


    [DisplayName("ИНН")]
    [JsonProperty("inn")]
    public string? INN { get; set; }

    [DisplayName("р/с")]
    [JsonProperty("bankAccount")]
    public string? BankAccount { get; set; }

    [DisplayName("Банк")]
    [JsonProperty("bank")]
    public string? Bank { get; set; }
  }
}