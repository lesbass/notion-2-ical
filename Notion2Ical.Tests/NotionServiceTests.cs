namespace Notion2Ical.Tests;

public class NotionServiceTests
{
    [Fact]
    public async Task GetVCalendarData_MovesOverdueTasksToTodayUntilTheyAreDone()
    {
        var repository = new StubNotionRepository(
            CreateTask(
                "ad275ad2-3b9d-481c-a067-75a10bb2f24c",
                "2000-01-01",
                "3d",
                "Pay taxes"));
        var service = new NotionService(repository, new NotionCalendarOptions());

        var calendar = await service.GetVCalendarData();

        var today = DateTime.Today;
        Assert.Contains($"DTSTART;VALUE=DATE:{today:yyyyMMdd}", calendar);
        Assert.Contains($"DTEND;VALUE=DATE:{today.AddDays(1):yyyyMMdd}", calendar);
        Assert.Contains("SUMMARY:3d Pay taxes", calendar);
        Assert.Contains("UID:ad275ad2-3b9d-481c-a067-75a10bb2f24c@Notion Tasks", calendar);
        Assert.Contains("URL:https://www.notion.so/ad275ad23b9d481ca06775a10bb2f24c", calendar);
    }

    [Fact]
    public async Task GetVCalendarData_SkipsTasksWithoutDueDate()
    {
        var repository = new StubNotionRepository(
            new Result
            {
                Id = "missing-due-date",
                Properties = new Properties()
            });
        var service = new NotionService(repository, new NotionCalendarOptions());

        var calendar = await service.GetVCalendarData();

        Assert.DoesNotContain("BEGIN:VEVENT", calendar);
    }

    [Fact]
    public async Task GetVCalendarData_UsesNotionDateEndWhenPresent()
    {
        var repository = new StubNotionRepository(
            CreateTask(
                "future-range",
                DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                "1d",
                "Book flight",
                DateTime.Today.AddDays(9).ToString("yyyy-MM-dd")));
        var service = new NotionService(repository, new NotionCalendarOptions());

        var calendar = await service.GetVCalendarData();

        Assert.Contains($"DTSTART;VALUE=DATE:{DateTime.Today.AddDays(7):yyyyMMdd}", calendar);
        Assert.Contains($"DTEND;VALUE=DATE:{DateTime.Today.AddDays(9):yyyyMMdd}", calendar);
    }

    private static Result CreateTask(string id, string start, string prefix, string title, string? end = null)
    {
        return new Result
        {
            Id = id,
            Properties = new Properties
            {
                Due = new Properties.DueDate
                {
                    Date = new Properties.DueDate.ItemDate
                    {
                        Start = start,
                        End = end
                    }
                },
                Priority = new Properties.Todo
                {
                    Formula = new Properties.Todo.ItemFormula
                    {
                        StringValue = prefix
                    }
                },
                Name = new Name
                {
                    Title = new List<Name.ItemTitle>
                    {
                        new()
                        {
                            Text = new Name.ItemTitle.ItemText
                            {
                                Content = title
                            }
                        }
                    }
                }
            }
        };
    }

    private sealed class StubNotionRepository : INotionRepository
    {
        private readonly IReadOnlyList<Result> _tasks;

        public StubNotionRepository(params Result[] tasks)
        {
            _tasks = tasks;
        }

        public Task<IReadOnlyList<Result>> GetTodoTasks(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_tasks);
        }
    }
}
