namespace Notion2Ical.Concretes;

internal class NotionRepository : INotionRepository
{
    private readonly HttpClient _httpClient;
    private readonly NotionCalendarOptions _options;

    public NotionRepository()
        : this(new NotionCalendarOptions())
    {
    }

    public NotionRepository(string accessToken, string databaseId)
        : this(new NotionCalendarOptions
        {
            AccessToken = accessToken,
            DatabaseId = databaseId
        })
    {
    }

    public NotionRepository(NotionCalendarOptions options)
        : this(new HttpClient(), options)
    {
    }

    public NotionRepository(HttpClient httpClient, NotionCalendarOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? new NotionCalendarOptions();
        ConfigureHttpClient();
    }

    public async Task<IReadOnlyList<Result>> GetTodoTasks(CancellationToken cancellationToken = default)
    {
        EnsureConfigured();

        var endpoint = $"v1/databases/{_options.DatabaseId}/query";
        string? startCursor = null;
        var resultItems = new List<Result>();

        do
        {
            if (!string.IsNullOrEmpty(startCursor))
            {
                await Task.Delay(_options.PageDelay, cancellationToken);
            }

            var query = BuildOpenTasksQuery(startCursor);
            var response = await _httpClient.PostAsync(
                endpoint,
                new StringContent(query, Encoding.UTF8, "application/json"),
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<QueryResponse>(
                await response.Content.ReadAsStringAsync(cancellationToken));

            if (result?.Results != null)
            {
                resultItems.AddRange(result.Results);
            }

            startCursor = result is { HasMore: true } ? result.NextCursor : null;
        } while (!string.IsNullOrEmpty(startCursor));

        return resultItems;
    }

    private string BuildOpenTasksQuery(string? startCursor)
    {
        var query = new Dictionary<string, object?>
        {
            ["page_size"] = _options.PageSize,
            ["filter"] = new
            {
                property = _options.StatusPropertyName,
                select = new
                {
                    does_not_equal = _options.DoneStatus
                }
            }
        };

        if (!string.IsNullOrEmpty(startCursor))
        {
            query["start_cursor"] = startCursor;
        }

        return JsonSerializer.Serialize(query);
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = _options.ApiBaseUrl;

        if (!string.IsNullOrWhiteSpace(_options.AccessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.AccessToken);
        }

        if (!_httpClient.DefaultRequestHeaders.Contains("Notion-Version"))
        {
            _httpClient.DefaultRequestHeaders.Add("Notion-Version", _options.NotionVersion);
        }
    }

    private void EnsureConfigured()
    {
        if (string.IsNullOrWhiteSpace(_options.AccessToken))
        {
            throw new InvalidOperationException("A Notion access token is required.");
        }

        if (string.IsNullOrWhiteSpace(_options.DatabaseId))
        {
            throw new InvalidOperationException("A Notion database id is required.");
        }

        if (_options.PageSize is < 1 or > 100)
        {
            throw new InvalidOperationException("Notion page size must be between 1 and 100.");
        }
    }
}
