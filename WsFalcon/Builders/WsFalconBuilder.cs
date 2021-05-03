namespace WsFalcon.Builders
{
    using Microsoft.Extensions.DependencyInjection;
    using Serializers.Abstract;

    public class WsFalconBuilder
    {
        public WsFalconBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public WsFalconBuilder AddDataSerializer<TSerializer>()
            where TSerializer : ISerializer
        {
            Services.AddSingleton(typeof(ISerializer), typeof(TSerializer));

            return this;
        }
    }
}