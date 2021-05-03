namespace WsFalcon.Extensions
{
    using Builders;
    using Microsoft.Extensions.DependencyInjection;
    using Serializers;
    using Serializers.Abstract;
    using Storages;
    using Storages.Abstract;
    using WsHandlers;

    public static class ServiceCollectionExtensions
    {
        public static WsFalconBuilder AddWsFalcon(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IWsSessionStorage, InMemoryWsSessionStorage>()
                .AddSingleton<ISerializer, JsonSerializer>()
                .AddScoped(typeof(WsHandlerLifeTimeManger<>));

            return new WsFalconBuilder(serviceCollection);
        }
    }
}