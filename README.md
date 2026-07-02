# notion-2-ical

.NET Standard library that turns a Notion task database into an iCalendar feed.

The feed can be exposed by an ASP.NET application and subscribed from calendar
apps such as Google Calendar, Outlook, Apple Calendar, or any client that reads
`.ics` feeds.

## Why this still exists

Notion Calendar can show Notion databases inside Notion Calendar, but it does
not expose a Notion database as a generic iCal feed for Google Calendar, iCloud,
Outlook, or Fantastical. This library is meant for that self-hosted use case.

## Behavior

- Reads tasks from a Notion database.
- Includes every task whose `Status` is not `Done 🙌`.
- Uses the Notion `Due Date` property as the calendar date.
- Uses the `Priority` formula string as a summary prefix. This can contain your
  countdown/priority text.
- Keeps overdue tasks visible by moving them to today until they are marked
  done.
- Generates stable event UIDs from Notion page IDs.

## Expected Notion database

The current models expect these properties:

| Property | Type | Purpose |
| --- | --- | --- |
| `Name` | Title | Event title |
| `Due Date` | Date | Event start date/time |
| `Priority` | Formula returning string | Prefix shown before the title |
| `Status` | Select | Used to hide completed tasks |

`Status` and the done value can be customized through `NotionCalendarOptions`.
The other property names are currently mapped in the model classes.

## Configuration

Create a Notion internal integration, share the database with it, and configure
the library with the integration token and database ID.

```csharp
var options = new NotionCalendarOptions
{
    AccessToken = "<notion-integration-token>",
    DatabaseId = "<notion-database-id>",
    CalendarName = "Notion Tasks",
    DoneStatus = "Done 🙌"
};
```

## ASP.NET Core setup

```csharp
services.AddSingleton(new NotionCalendarOptions
{
    AccessToken = Configuration["Notion:AccessToken"],
    DatabaseId = Configuration["Notion:DatabaseId"],
    CalendarName = "Notion Tasks"
});

services.AddHttpClient<INotionRepository, NotionRepository>();
services.AddTransient<INotionService, NotionService>();
```

## Controller example

```csharp
using System.Threading;
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

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            Response.Headers.Add("Content-Disposition", "inline; filename=notion-tasks.ics");
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            var calendar = await _notionService.GetVCalendarData(cancellationToken);
            return Content(calendar, "text/calendar");
        }
    }
}
```

## Notion API

- API reference: https://developers.notion.com/
- Database query API: https://developers.notion.com/reference/post-database-query

The repository sends `Notion-Version: 2022-06-28` by default.
