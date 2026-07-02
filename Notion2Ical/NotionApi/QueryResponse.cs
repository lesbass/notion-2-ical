namespace Notion2Ical.NotionApi;

public class QueryResponse
{
    [JsonPropertyName("results")]
    public List<Result>? Results { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }
}
