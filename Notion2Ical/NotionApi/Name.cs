using System.Collections.Generic;
using Newtonsoft.Json;

namespace Notion.Models
{
    public class Name
    {
        [JsonProperty("title")] public List<ItemTitle> Title { get; set; }

        public class ItemTitle
        {
            [JsonProperty("text")] public ItemText Text { get; set; }

            public class ItemText
            {
                [JsonProperty("content")] public string Content { get; set; }
            }
        }
    }
}