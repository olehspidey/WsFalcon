namespace WsFalcon.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Storages;
    using Storages.Abstract;
    using WsHandlers;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWsFalcon(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IWsStorage, InMemoryWsStorage>()
                .AddScoped(typeof(WsHandlerLifeTimeManger<>));
        }
    }
}