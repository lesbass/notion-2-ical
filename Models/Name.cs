using System.Collections.Generic;

namespace Notion.Models
{
    public class Name
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<Title> title { get; set; }

        public class Title
        {
            public string type { get; set; }
            public Text text { get; set; }
            public Annotations annotations { get; set; }
            public string plain_text { get; set; }
            public object href { get; set; }

            public class Text
            {
                public string content { get; set; }
                public object link { get; set; }
            }
            public class Annotations
            {
                public bool bold { get; set; }
                public bool italic { get; set; }
                public bool strikethrough { get; set; }
                public bool underline { get; set; }
                public bool code { get; set; }
                public string color { get; set; }
            }
        }
    }
}