using System.Threading.Tasks;

namespace Notion.Interfaces
{
    public interface INotionService
    {
        Task<string> GetVCalendarData();
    }
}