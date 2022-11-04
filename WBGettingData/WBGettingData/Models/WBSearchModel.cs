using System.Text.Json.Serialization;

namespace WBGettingData.Models
{
    public class WBSearchModel
    {
        [JsonPropertyName("state")]
        public int State { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("data")]
        public Data? Data { get; set; }
    }
}
