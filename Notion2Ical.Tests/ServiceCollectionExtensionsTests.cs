namespace Notion2Ical.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddNotionCalendarFeed_RegistersLibraryServices()
    {
        var services = new ServiceCollection();
        var options = new NotionCalendarOptions
        {
            AccessToken = "secret-token",
            DatabaseId = "database-id"
        };

        var returnedServices = services.AddNotionCalendarFeed(options);

        Assert.Same(services, returnedServices);

        using var provider = services.BuildServiceProvider();
        Assert.Same(options, provider.GetRequiredService<NotionCalendarOptions>());
        Assert.IsAssignableFrom<INotionRepository>(provider.GetRequiredService<INotionRepository>());
        Assert.IsAssignableFrom<INotionService>(provider.GetRequiredService<INotionService>());
    }
}
