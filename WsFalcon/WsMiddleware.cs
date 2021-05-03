namespace WsFalcon
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using WsHandlers;
    using WsHandlers.Abstract;

    public class WsMiddleware<TWsHandler> : IMiddleware
        where TWsHandler : WsHandlerBase
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var serviceScope = context.RequestServices.CreateScope();
                var wsLifeTimeManger = serviceScope.ServiceProvider.GetRequiredService<WsHandlerLifeTimeManger<TWsHandler>>();

                var ws = await context.WebSockets.AcceptWebSocketAsync();

                await wsLifeTimeManger.HandleSocketAccepted(ws, context);

                return;
            }

            await next(context);
        }
    }
}