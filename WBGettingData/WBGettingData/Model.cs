using System.Text.Json.Serialization;

namespace WBGettingData;

public class Model
{
    [JsonPropertyName("state")]
    public int State { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }
}

public class Product
{
    [JsonPropertyName("brand")]
    public string Brand { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}

public class Data
{
    [JsonPropertyName("products")]
    public Product[] Products { get; set; }
}