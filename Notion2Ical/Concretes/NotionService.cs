namespace Notion2Ical.Concretes;

internal class NotionService : INotionService
{
    private readonly INotionRepository _notionRepository;
    private readonly NotionCalendarOptions _options;

    public NotionService(INotionRepository notionRepository)
        : this(notionRepository, new NotionCalendarOptions())
    {
    }

    public NotionService(INotionRepository notionRepository, NotionCalendarOptions options)
    {
        _notionRepository = notionRepository ?? throw new ArgumentNullException(nameof(notionRepository));
        _options = options ?? new NotionCalendarOptions();
    }

    public async Task<string> GetVCalendarData(CancellationToken cancellationToken = default)
    {
        var calendar = new VCalendar(_options.CalendarName);
        var tasks = await _notionRepository.GetTodoTasks(cancellationToken);

        foreach (var task in tasks)
        {
            var taskEvent = TryGetTaskEvent(task);
            if (taskEvent != null)
            {
                calendar.AddEvent(taskEvent);
            }
        }

        return calendar.ToString();
    }

    private VEvent? TryGetTaskEvent(Result task)
    {
        var dueDateValue = task.Properties?.Due?.Date?.Start;
        if (string.IsNullOrWhiteSpace(dueDateValue) || string.IsNullOrWhiteSpace(task.Id))
        {
            return null;
        }

        var itemId = task.Id;
        var itemUrl = new Uri(_options.PageBaseUrl, itemId.Replace("-", string.Empty)).ToString();
        var hasTime = dueDateValue.Contains('T');
        var dueDate = DateTime.Parse(dueDateValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        var isOverdue = dueDate < DateTime.Now;

        if (isOverdue)
        {
            dueDate = DateTime.Today;
            hasTime = false;
        }

        var end = GetEndDate(task, dueDate, hasTime, isOverdue);

        var title =
            $"{task.Properties?.Priority?.Formula?.StringValue} {task.Properties?.Name?.Title?.FirstOrDefault()?.Text?.Content}"
                .Trim();

        var description = $"Url: {itemUrl}";

        return new VEvent
        (
            itemId,
            dueDate,
            end,
            hasTime,
            title,
            description,
            itemUrl
        );
    }

    private static DateTime GetEndDate(Result task, DateTime start, bool hasTime, bool isOverdue)
    {
        var endDateValue = task.Properties?.Due?.Date?.End;
        if (!isOverdue && !string.IsNullOrWhiteSpace(endDateValue))
        {
            return DateTime.Parse(endDateValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        return hasTime
            ? start.AddHours(1)
            : start.AddDays(1);
    }
}
