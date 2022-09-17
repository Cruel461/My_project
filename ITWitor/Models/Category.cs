using ITWitor.Models;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations.Schema;

namespace ITWitor.Models
{
  public class Category : BaseModel
    {
        [JsonIgnore]
        public Category? Parent { get; set; }

        [ForeignKey("Parent")]
        [JsonProperty("parentId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ParentId { get; set; }

        [JsonProperty("categories", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Category[]? Categories { get; set; }

        [JsonProperty("products", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Product[]? Products { get; set; }

        [JsonProperty("imagePath", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ImagePath { get; set; }
    }
}