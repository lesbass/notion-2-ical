namespace Notion2Ical.Tests;

public class NotionRepositoryTests
{
    [Fact]
    public async Task GetTodoTasks_QueriesOpenTasksAcrossPages()
    {
        var handler = new CapturingHandler(
            @"{ ""results"": [{ ""id"": ""first"", ""properties"": {} }], ""has_more"": true, ""next_cursor"": ""cursor-1"" }",
            @"{ ""results"": [{ ""id"": ""second"", ""properties"": {} }], ""has_more"": false }");

        var options = new NotionCalendarOptions
        {
            AccessToken = "secret-token",
            DatabaseId = "database-id",
            DoneStatus = "Archived",
            PageDelay = TimeSpan.Zero
        };
        var repository = new NotionRepository(new HttpClient(handler), options);

        var tasks = await repository.GetTodoTasks();

        Assert.Equal(new[] { "first", "second" }, tasks.Select(task => task.Id));
        Assert.Equal(2, handler.Requests.Count);
        Assert.All(handler.Requests, request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://api.notion.com/v1/databases/database-id/query", request.RequestUri?.ToString());
            Assert.Equal(new AuthenticationHeaderValue("Bearer", "secret-token"), request.Headers.Authorization);
            Assert.True(request.Headers.TryGetValues("Notion-Version", out var notionVersions));
            Assert.Equal("2022-06-28", Assert.Single(notionVersions));
        });

        Assert.Contains(@"""does_not_equal"":""Archived""", handler.Bodies[0]);
        Assert.DoesNotContain("start_cursor", handler.Bodies[0]);
        Assert.Contains(@"""start_cursor"":""cursor-1""", handler.Bodies[1]);
    }

    [Fact]
    public async Task GetTodoTasks_RequiresConfiguration()
    {
        var repository = new NotionRepository(new HttpClient(new CapturingHandler()), new NotionCalendarOptions());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.GetTodoTasks());

        Assert.Equal("A Notion access token is required.", exception.Message);
    }

    private sealed class CapturingHandler : HttpMessageHandler
    {
        private readonly Queue<string> _responses;

        public CapturingHandler(params string[] responses)
        {
            _responses = new Queue<string>(responses);
        }

        public List<string> Bodies { get; } = new();

        public List<HttpRequestMessage> Requests { get; } = new();

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Requests.Add(request);
            Bodies.Add(request.Content == null ? string.Empty : await request.Content.ReadAsStringAsync());

            var responseBody = _responses.Count > 0
                ? _responses.Dequeue()
                : @"{ ""results"": [], ""has_more"": false }";

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody)
            };
        }
    }
}
