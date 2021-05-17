using System;
using System.Linq;
using System.Threading.Tasks;
using Notion.Interfaces;
using Notion.Models;

namespace Notion
{
    public class NotionService : INotionService
    {
        private const string BaseNotionUrl = "https://www.notion.so/";
        private readonly INotionRepository _notionRepository;

        public NotionService(INotionRepository notionRepository)
        {
            _notionRepository = notionRepository;
        }

        public async Task<string> GetVCalendarData()
        {
            var calendar = new VCalendar("Notion Tasks");

            (await _notionRepository.GetTodoTasks()).ForEach(task =>
                {
                    try
                    {
                        calendar.AddEvent(GetTaskEvent(task));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            );

            return calendar.ToString();
        }


        private VEvent GetTaskEvent(Result task)
        {
            var dateFormat = "yyyyMMddTHHmmssZ";

            var itemId = task.Id;
            var itemUrl = $"{BaseNotionUrl}{itemId.Replace("-", "")}";
            var dueDate = DateTime.Parse(task.Properties.Due.Date.Start);
            var hasHours = dueDate.Hour > 0;
            if (dueDate < DateTime.Now) dueDate = DateTime.Now.Date;

            var start = hasHours
                ? "DTSTART:" + dueDate.ToUniversalTime().ToString(dateFormat)
                : $"DTSTART;VALUE=DATE:{dueDate:yyyyMMdd}";
            var end = hasHours
                ? "DTEND:" + dueDate.AddHours(1).ToUniversalTime().ToString(dateFormat)
                : $"DTEND;VALUE=DATE:{dueDate.AddDays(1):yyyyMMdd}";

            var title =
                $"{task.Properties.Priority.Formula.StringValue} {task.Properties.Name.Title.FirstOrDefault()?.Text.Content}";

            var description = $"Url: {itemUrl}";

            return new VEvent
            (
                start,
                end,
                title,
                description,
                itemUrl
            );
        }
    }
}