using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Notion.Interfaces;
using Notion.Models;

namespace Notion
{
    public class NotionRepository : INotionRepository
    {
        private const string BaseUrl = "https://api.notion.com";
        private const string AccessToken = "";
        private const string TaskDatabaseId = "";
        private readonly HttpClient _httpClient;

        public NotionRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
        }

        public async Task<List<Result>> GetTodoTasks()
        {
            var endpoint = $"{BaseUrl}/v1/databases/{TaskDatabaseId}/query";

            var query = @"{
    ""filter"": {
        ""property"": ""Status"",
        ""select"": {
            ""does_not_equal"": ""Done 🙌""
        }
    }
}";
            var startCursor = "";
            var resultItems = new List<Result>();
            do
            {
                var localEndpoint = endpoint;
                if (!string.IsNullOrEmpty(startCursor))
                {
                    localEndpoint = $"{endpoint}?start_cursor={startCursor}";
                    Thread.Sleep(500);
                }
                var response = _httpClient.PostAsync(localEndpoint, new StringContent(query, Encoding.UTF8, "application/json"));
                var result =
                    JsonConvert.DeserializeObject<QueryResponse>(await response.Result.Content.ReadAsStringAsync());
                resultItems.AddRange(result.Results);

                startCursor = result.HasMore ? result.NextCursor : string.Empty;
            } while (!string.IsNullOrEmpty(startCursor));

            return resultItems;
        }
    }
}