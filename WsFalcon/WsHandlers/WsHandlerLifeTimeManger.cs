namespace WsFalcon.WsHandlers
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using Abstract;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Serializers.Abstract;
    using Storages.Abstract;

    public class WsHandlerLifeTimeManger<TWsHandler>
        where TWsHandler : WsHandlerBase
    {
        private readonly TWsHandler _wsHandler;

        public WsHandlerLifeTimeManger(IServiceProvider serviceProvider)
        {
            _wsHandler = ActivatorUtilities.GetServiceOrCreateInstance<TWsHandler>(serviceProvider);
        }

        public async Task HandleSocketAccepted(WebSocket webSocket, HttpContext httpContext)
        {
            try
            {
                await OnConnectedAsync(httpContext, webSocket);

                var arrSegment = new ArraySegment<byte>(new byte[1024 * 4]);
                var receiveResult = await webSocket.ReceiveAsync(arrSegment, httpContext.RequestAborted);

                await _wsHandler.OnMessageAsync(arrSegment[..receiveResult.Count]);

                while (!webSocket.CloseStatus.HasValue)
                {
                    receiveResult = await webSocket.ReceiveAsync(arrSegment, httpContext.RequestAborted);

                    await _wsHandler.OnMessageAsync(arrSegment[..receiveResult.Count]);
                }

                await OnDisconnectedAsync(httpContext, webSocket.CloseStatus, webSocket.CloseStatusDescription);
            }
            catch (Exception e)
            {
                await OnDisconnectedAsync(httpContext, webSocket.CloseStatus, webSocket.CloseStatusDescription);
            }
        }

        private Task OnConnectedAsync(HttpContext httpContext, WebSocket webSocket)
        {
            _wsHandler.CurrentWebSocket = webSocket;
            _wsHandler.WsStorage = httpContext.RequestServices.GetRequiredService<IWsStorage>();
            _wsHandler.Serializer = httpContext.RequestServices.GetRequiredService<ISerializer>();

            return _wsHandler.OnConnectedAsync(httpContext);
        }

        private Task OnDisconnectedAsync(
            HttpContext httpContext,
            WebSocketCloseStatus? webSocketCloseStatus,
            string wsCloseStatusDescription,
            Exception? exception = null)
            => _wsHandler.OnDisconnectedAsync(httpContext, webSocketCloseStatus, wsCloseStatusDescription, exception);
    }
}