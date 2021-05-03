namespace WsFalcon.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using WsHandlers.Abstract;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWsHandler<THandler>(this IApplicationBuilder applicationBuilder, string path)
        where THandler : WsHandlerBase
        {
            return applicationBuilder
                .UseWebSockets()
                .Use((context, func) => new WsMiddleware<THandler>().InvokeAsync(context, httpContext => func()));
        }
    }
}