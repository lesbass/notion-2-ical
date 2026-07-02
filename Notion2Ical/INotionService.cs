namespace Notion2Ical;

public interface INotionService
{
    Task<string> GetVCalendarData(CancellationToken cancellationToken = default);
}
