using System;
using Newtonsoft.Json;

namespace Notion.Models
{
    public class Properties
    {
        [JsonProperty("Date Created")]
        public DateCreated Created { get; set; }
        [JsonProperty("Due Date")]
        public DueDate Due { get; set; }
        [JsonProperty("Status")]
        public Status CurrentStatus { get; set; }

        [JsonProperty("🕰")] public Todo Priority { get; set; }

        [JsonProperty("Name")]
        public Name Name { get; set; }

        public class Status
        {
            public string id { get; set; }
            public string type { get; set; }
            public Select select { get; set; }
            public class Select
            {
                public string id { get; set; }
                public string name { get; set; }
                public string color { get; set; }
            }
        }
        public class Todo
        {
            public string id { get; set; }
            public string type { get; set; }
            public Formula formula { get; set; }
            public class Formula
            {
                public string type { get; set; }
                public string @string { get; set; }
                public bool boolean { get; set; }
                public int number { get; set; }
            }
        }
        public class DueDate
        {
            public string id { get; set; }
            public string type { get; set; }
            public Date date { get; set; }
            public class Date
            {
                public string start { get; set; }
                public string end { get; set; }
            }
        }
        public class DateCreated
        {
            public string id { get; set; }
            public string type { get; set; }
            public DateTime created_time { get; set; }
        }
    }
}