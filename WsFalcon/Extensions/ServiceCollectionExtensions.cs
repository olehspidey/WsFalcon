namespace WsFalcon.Extensions
{
    using System;
    using Builders;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using Serializers;
    using Serializers.Abstract;
    using Storages;
    using Storages.Abstract;
    using WsHandlers;

    public static class ServiceCollectionExtensions
    {
        public static WsFalconServiceBuilder AddWsFalcon(this IServiceCollection serviceCollection, Action<WsFalconOptions>? config = null)
        {
            serviceCollection
                .AddSingleton<IWsSessionStorage, InMemoryWsSessionStorage>()
                .AddSingleton<ISerializer, JsonSerializer>()
                .AddScoped(typeof(WsHandlerLifeTimeManger<>));

            if (config != null)
                serviceCollection.Configure(config);

            return new WsFalconServiceBuilder(serviceCollection);
        }
    }
}