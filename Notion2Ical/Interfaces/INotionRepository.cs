using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Notion.Models;

namespace Notion.Interfaces
{
    public interface INotionRepository
    {
        Task<IReadOnlyList<Result>> GetTodoTasks(CancellationToken cancellationToken = default(CancellationToken));
    }
}
