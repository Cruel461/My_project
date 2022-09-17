using ITWitor.Models;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations.Schema;

namespace ITWitor.Models
{
  public class Product : BaseModel
    {
        [JsonIgnore]
        public Category? Category { get; set; }

        [ForeignKey("Category")]
        public int? ParentId { get; set; }
    }
}