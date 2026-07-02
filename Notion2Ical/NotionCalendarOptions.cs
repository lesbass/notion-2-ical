using System;

namespace Notion
{
    public class NotionCalendarOptions
    {
        public string AccessToken { get; set; }

        public string DatabaseId { get; set; }

        public string CalendarName { get; set; } = "Notion Tasks";

        public string DoneStatus { get; set; } = "Done 🙌";

        public string StatusPropertyName { get; set; } = "Status";

        public string NotionVersion { get; set; } = "2022-06-28";

        public Uri ApiBaseUrl { get; set; } = new Uri("https://api.notion.com");

        public Uri PageBaseUrl { get; set; } = new Uri("https://www.notion.so/");

        public int PageSize { get; set; } = 100;

        public TimeSpan PageDelay { get; set; } = TimeSpan.FromMilliseconds(500);
    }
}
