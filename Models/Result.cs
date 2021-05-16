using System;
using Newtonsoft.Json;

namespace Notion.Models
{
    public class Result
    {
        [JsonProperty("object")] public string ObjectType { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("created_time")] public DateTime CreatedTime { get; set; }

        [JsonProperty("last_edited_time")] public DateTime LastEditedTime { get; set; }

        [JsonProperty("archived")] public bool Archived { get; set; }

        [JsonProperty("properties")] public Properties Properties { get; set; }
    }
}