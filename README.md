# notion-2-ical
This is a small class library intended to be used with a web api/website in order to expose an iCal feed to be imported in apps such as Google Calendar or Microsoft Outlook.
It maps the way I arranged the structure of my task items in Notion, so it should be adapted based on your specific way of using Notion.

## API Reference
Here is a link to Notion APIs: https://developers.notion.com/

Specifically, Database query API: https://developers.notion.com/reference/post-database-query

## Controller Example
Using this in a .NET Core MVC environment, an example of a Controller could be:

<pre>
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notion.Interfaces;

namespace MyApp
{
    public class CalendarController : Controller
    {
        private readonly INotionService _notionService;

        public CalendarController(INotionService notionService)
        {
            _notionService = notionService;
        }

        public async Task<IActionResult> Index()
        {
            Response.Headers.Add("Content-Disposition", "attachment");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return Content(await _notionService.GetVCalendarData(), "text/calendar");
        }
    }
}
</pre>
