namespace Notion2Ical.NotionApi;

public class Properties
{
    [JsonPropertyName("Due Date")]
    public DueDate? Due { get; set; }

    [JsonPropertyName("Priority")]
    public Todo? Priority { get; set; }

    [JsonPropertyName("Name")]
    public Name? Name { get; set; }

    public class Todo
    {
        [JsonPropertyName("formula")]
        public ItemFormula? Formula { get; set; }

        public class ItemFormula
        {
            [JsonPropertyName("string")]
            public string? StringValue { get; set; }
        }
    }

    public class DueDate
    {
        [JsonPropertyName("date")]
        public ItemDate? Date { get; set; }

        public class ItemDate
        {
            [JsonPropertyName("start")]
            public string? Start { get; set; }

            [JsonPropertyName("end")]
            public string? End { get; set; }
        }
    }
}
