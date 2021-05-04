namespace WsFalcon
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using WsHandlers;
    using WsHandlers.Abstract;

    public class WsMiddleware<TWsHandler> : IMiddleware
        where TWsHandler : WsHandlerBase
    {
        private readonly string _path;

        private readonly ILogger<WsMiddleware<TWsHandler>> _logger;

        public WsMiddleware(string path, ILogger<WsMiddleware<TWsHandler>> logger)
        {
            _path = path;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.WebSockets.IsWebSocketRequest && context.Request.Path == _path)
            {
                _logger.LogInformation("Starting accept WS request");

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