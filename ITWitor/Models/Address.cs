

using ITWitor.Models;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations.Schema;

namespace ITWitor.Models
{
  public class Address : BaseModel
    {
        [JsonIgnore]
        public Contacts? Parent { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        [JsonProperty("postalCode", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? PostalCode { get; set; }

        [JsonProperty("city", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? City { get; set; }

        [JsonProperty("cityType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? CityType { get; set; }

        [JsonProperty("region", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Region { get; set; }

        [JsonProperty("regionType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? RegionType { get; set; }

        [JsonProperty("house", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? House { get; set; }

        [JsonProperty("street", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Street { get; set; }

        [JsonProperty("entrance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Entrance { get; set; }

        [JsonProperty("flat", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Flat { get; set; }

        [JsonProperty("floor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Floor { get; set; }

        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Text { get; set; }

        [JsonProperty("latitude", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Latitude { get; set; }

        [JsonProperty("longitude", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Longitude { get; set; }
    }
}
