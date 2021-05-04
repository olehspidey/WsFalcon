namespace WsFalcon.Builders
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Serializers.Abstract;

    public class WsFalconServiceBuilder
    {
        public WsFalconServiceBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public WsFalconServiceBuilder AddDataSerializer<TSerializer>()
            where TSerializer : ISerializer
        {
            Services.RemoveAll(typeof(ISerializer));
            Services.AddSingleton(typeof(ISerializer), typeof(TSerializer));

            return this;
        }
    }
}