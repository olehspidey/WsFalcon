namespace WsFalcon.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using WsHandlers.Abstract;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWsHandler<THandler>(this IApplicationBuilder applicationBuilder, string path)
            where THandler : WsHandlerBase
        {
            return applicationBuilder
                .UseWebSockets()
                .Use((context, func) => new WsMiddleware<THandler>(
                    path,
                    context.RequestServices.GetRequiredService<ILogger<WsMiddleware<THandler>>>()).InvokeAsync(context, _ => func()));
        }
    }
}