namespace Notion2Ical;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotionCalendarFeed(
        this IServiceCollection services,
        NotionCalendarOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        services.AddSingleton(options);
        services.AddHttpClient<INotionRepository, NotionRepository>();
        services.AddTransient<INotionService, NotionService>();

        return services;
    }
}
