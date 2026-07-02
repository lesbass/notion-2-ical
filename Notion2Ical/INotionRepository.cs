namespace Notion2Ical;

internal interface INotionRepository
{
    Task<IReadOnlyList<Result>> GetTodoTasks(CancellationToken cancellationToken = default);
}
