namespace Notion2Ical.NotionApi;

public class Result
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("properties")]
    public Properties? Properties { get; set; }
}
