using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notion.Interfaces;

namespace Notion
{
    public class NotionService : INotionService
    {
        private readonly INotionRepository _notionRepository;
        private const string BaseNotionUrl = "https://www.notion.so/";

        public NotionService(INotionRepository notionRepository)
        {
            _notionRepository = notionRepository;
        }

        public async Task<string> GetVCalendarData()
        {
            var sb = new StringBuilder();
            sb.Append("BEGIN:VCALENDAR");
            sb.Append("\r\nVERSION:2.0");
            sb.Append("\r\nNAME:Notion Tasks");
            sb.Append("\r\nX-WR-CALNAM:Notion Tasks");
            sb.Append("\r\nMETHOD:PUBLISH");

            var tasks = await _notionRepository.GetTodoTasks();
            var dateFormat = "yyyyMMddTHHmmssZ";
            foreach (var task in tasks)
            {
                var itemId = task.Id;
                var itemUrl = $"{BaseNotionUrl}{itemId.Replace("-", "")}";
                var dueDate = DateTime.Parse(task.Properties.Due.date.start);
                var hasHours = dueDate.Hour > 0;
                if(dueDate < DateTime.Now) dueDate = DateTime.Now.Date;

                var start = hasHours
                    ? "DTSTART:" + dueDate.ToUniversalTime().ToString(dateFormat)
                    : $"DTSTART;VALUE=DATE:{dueDate:yyyyMMdd}";
                var end = hasHours
                    ? "DTEND:" + dueDate.AddHours(1).ToUniversalTime().ToString(dateFormat)
                    : $"DTEND;VALUE=DATE:{dueDate.AddDays(1):yyyyMMdd}";

                var title =
                    $"{task.Properties.Priority.formula.@string} {task.Properties.Name.title.FirstOrDefault()?.text.content}";

                var description = $"Url: {itemUrl}";

                sb.Append("\r\nBEGIN:VEVENT");
                sb.Append($"\r\n{start}");
                sb.Append($"\r\n{end}");
                sb.Append($"\r\nUID:{itemId}");
                sb.Append($"\r\nURL:{itemUrl}");
                sb.Append($"\r\nDTSTAMP:{DateTime.Now.ToUniversalTime().ToString(dateFormat)}");
                sb.Append($"\r\nSUMMARY:{title}");
                sb.Append($"\r\nDESCRIPTION:{description}");
                sb.Append("\r\nPRIORITY:5");
                sb.Append("\r\nCLASS:PUBLIC");
                sb.Append("\r\nEND:VEVENT");
            }

            sb.Append("\r\nEND:VCALENDAR");

            return sb.ToString();
        }
    }
}