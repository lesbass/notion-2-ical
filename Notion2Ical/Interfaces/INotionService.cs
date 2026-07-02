namespace Notion2Ical.Interfaces;

public interface INotionService
{
    Task<string> GetVCalendarData(CancellationToken cancellationToken = default);
}
