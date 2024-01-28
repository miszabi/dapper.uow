using Microsoft.Extensions.DependencyInjection;

namespace Dev;

public static class ServiceExtensions
{

    public static void Register(this IServiceCollection serviceCollection,  Action<AppConfig> action)
    {
        AppConfig config = new AppConfig();

        action(config);

        if(string.IsNullOrWhiteSpace(config.ConnectionString)) throw new ArgumentNullException(nameof(config.ConnectionString));

        serviceCollection.AddSingleton(config);
        serviceCollection.AddSingleton<IConnectionProvider, SQLConnectionProvider>();
        serviceCollection.AddSingleton<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddSingleton<IRepository, Repostitory>();
    }
}
