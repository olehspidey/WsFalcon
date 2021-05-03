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
    using WebSocketContext = WebSocketContext;

    public class WsHandlerLifeTimeManger<TWsHandler>
        where TWsHandler : WsHandlerBase
    {
        private readonly TWsHandler _wsHandler;
        private readonly Type _wsHandlerType;

        public WsHandlerLifeTimeManger(IServiceProvider serviceProvider)
        {
            _wsHandler = ActivatorUtilities.GetServiceOrCreateInstance<TWsHandler>(serviceProvider);
            _wsHandlerType = typeof(TWsHandler);
        }

        public async Task HandleSocketAccepted(WebSocket webSocket, HttpContext httpContext)
        {
            try
            {
                await OnConnectedAsync(httpContext, webSocket);

                var arrSegment = new ArraySegment<byte>(new byte[1024 * 4]);
                var receiveResult = await webSocket.ReceiveAsync(arrSegment, httpContext.RequestAborted);

                if (receiveResult.CloseStatus.HasValue)
                {
                    await OnDisconnectedAsync(webSocket.CloseStatus, webSocket.CloseStatusDescription);

                    return;
                }

                await _wsHandler.OnMessageAsync(arrSegment[..receiveResult.Count]);

                while (!webSocket.CloseStatus.HasValue)
                {
                    receiveResult = await webSocket.ReceiveAsync(arrSegment, httpContext.RequestAborted);

                    if (receiveResult.CloseStatus.HasValue)
                    {
                        await OnDisconnectedAsync(webSocket.CloseStatus, webSocket.CloseStatusDescription);

                        return;
                    }

                    await _wsHandler.OnMessageAsync(arrSegment[..receiveResult.Count]);
                }

                await OnDisconnectedAsync(webSocket.CloseStatus, webSocket.CloseStatusDescription);
            }
            catch (Exception e)
            {
                await OnDisconnectedAsync(webSocket.CloseStatus, webSocket.CloseStatusDescription);
            }
        }

        private Task OnConnectedAsync(HttpContext httpContext, WebSocket webSocket)
        {
            _wsHandler.CurrentWebSocket = webSocket;
            _wsHandler.WsSessionStorage = httpContext.RequestServices.GetRequiredService<IWsSessionStorage>();
            _wsHandler.Serializer = httpContext.RequestServices.GetRequiredService<ISerializer>();
            _wsHandler.WsSessionStorage.SaveWebSocket(_wsHandlerType, webSocket);
            _wsHandler.WsContext = new WebSocketContext(httpContext.Connection);

            return _wsHandler.OnConnectedAsync();
        }

        private Task OnDisconnectedAsync(
            WebSocketCloseStatus? webSocketCloseStatus,
            string wsCloseStatusDescription,
            Exception? exception = null)
        {
            _wsHandler.WsSessionStorage.Delete(_wsHandlerType, _wsHandler.CurrentWebSocket);

            return _wsHandler.OnDisconnectedAsync(webSocketCloseStatus, wsCloseStatusDescription, exception);
        }
    }
}