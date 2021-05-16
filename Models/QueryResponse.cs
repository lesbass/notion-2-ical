using System.Collections.Generic;
using Newtonsoft.Json;

namespace Notion.Models
{
    public class QueryResponse
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }
    }
}