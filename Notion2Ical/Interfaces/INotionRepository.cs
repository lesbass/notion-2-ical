namespace Notion2Ical.Interfaces;

public interface INotionRepository
{
    Task<IReadOnlyList<Result>> GetTodoTasks(CancellationToken cancellationToken = default);
}
