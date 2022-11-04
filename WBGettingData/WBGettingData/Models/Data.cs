using System.Text.Json.Serialization;

namespace WBGettingData.Models
{
    public class Data
    {
        [JsonPropertyName("products")]
        public WBSearchProduct[]? Products { get; set; }
    }
}
