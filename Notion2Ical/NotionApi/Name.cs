namespace Notion2Ical.NotionApi;

internal class Name
{
    [JsonPropertyName("title")]
    public List<ItemTitle>? Title { get; set; }

    public class ItemTitle
    {
        [JsonPropertyName("text")]
        public ItemText? Text { get; set; }

        public class ItemText
        {
            [JsonPropertyName("content")]
            public string? Content { get; set; }
        }
    }
}
