using System.Collections.Generic;
using System.Threading.Tasks;
using Notion.Models;

namespace Notion.Interfaces
{
    public interface INotionRepository
    {
        Task<List<Result>> GetTodoTasks();
    }
}