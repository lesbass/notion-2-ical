using Newtonsoft.Json;

namespace Notion.Models
{
    public class Result
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("properties")] public Properties Properties { get; set; }
    }
}