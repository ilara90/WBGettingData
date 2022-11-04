using System.Text.Json.Serialization;

namespace WBGettingData.Models
{
    public class WBSearchProduct
    {
        [JsonPropertyName("priceU")]
        public int? Price { get; set; }

        [JsonPropertyName("feedbacks")]
        public int? Feddbacks { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
