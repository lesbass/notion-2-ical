using System.Threading;
using System.Threading.Tasks;

namespace Notion.Interfaces
{
    public interface INotionService
    {
        Task<string> GetVCalendarData(CancellationToken cancellationToken = default(CancellationToken));
    }
}
