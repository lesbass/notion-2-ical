using Newtonsoft.Json;

namespace Notion.Models
{
    public class Properties
    {
        [JsonProperty("Due Date")] public DueDate Due { get; set; }

        [JsonProperty("Priority")] public Todo Priority { get; set; }

        [JsonProperty("Name")] public Name Name { get; set; }

        public class Todo
        {
            [JsonProperty("formula")] public ItemFormula Formula { get; set; }

            public class ItemFormula
            {
                [JsonProperty("string")] public string StringValue { get; set; }
            }
        }

        public class DueDate
        {
            [JsonProperty("date")] public ItemDate Date { get; set; }

            public class ItemDate
            {
                [JsonProperty("start")] public string Start { get; set; }
            }
        }
    }
}