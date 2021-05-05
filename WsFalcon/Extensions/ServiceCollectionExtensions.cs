namespace WsFalcon.Extensions
{
    using System;
    using Builders;
    using Managers;
    using Managers.Abstract.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using Serializers;
    using Serializers.Abstract;
    using WsHandlers;

    public static class ServiceCollectionExtensions
    {
        public static WsFalconServiceBuilder AddWsFalcon(this IServiceCollection serviceCollection, Action<WsFalconOptions>? config = null)
        {
            serviceCollection
                .AddSingleton(typeof(IWsSessionsManager<>), typeof(WsSessionsManager<>))
                .AddSingleton(typeof(IInternalGroupManager<>), typeof(WsGroupManager<>))
                .AddSingleton<ISerializer, JsonSerializer>()
                .AddScoped(typeof(WsHandlerLifeTimeManger<>));

            if (config != null)
                serviceCollection.Configure(config);

            return new WsFalconServiceBuilder(serviceCollection);
        }
    }
}