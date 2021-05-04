namespace WsFalcon.WsHandlers
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using Abstract;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Options;
    using Serializers.Abstract;
    using Storages.Abstract;
    using WebSocketContext = WebSocketContext;

    public class WsHandlerLifeTimeManger<TWsHandler>
        where TWsHandler : WsHandlerBase
    {
        private readonly TWsHandler _wsHandler;
        private readonly Type _wsHandlerType;
        private readonly ILogger<WsHandlerLifeTimeManger<TWsHandler>> _logger;
        private readonly int _bufferSize;

        public WsHandlerLifeTimeManger(IServiceProvider serviceProvider)
        {
            _wsHandler = ActivatorUtilities.GetServiceOrCreateInstance<TWsHandler>(serviceProvider);
            _wsHandlerType = typeof(TWsHandler);
            _logger = serviceProvider.GetRequiredService<ILogger<WsHandlerLifeTimeManger<TWsHandler>>>();
            _bufferSize = serviceProvider
                .GetRequiredService<IOptions<WsFalconOptions>>()
                .Value
                .WsBufferSize ?? 1024 * 4;
        }

        public async Task HandleSocketAccepted(WebSocket webSocket, HttpContext httpContext)
        {
            try
            {
                await OnConnectedAsync(httpContext, webSocket);

                var arrSegment = new ArraySegment<byte>(new byte[_bufferSize]);
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
                _logger.LogError(e, $"Receive message error for {_wsHandlerType.FullName}");
                await OnDisconnectedAsync(webSocket.CloseStatus, webSocket.CloseStatusDescription);
            }
        }

        private Task OnConnectedAsync(HttpContext httpContext, WebSocket webSocket)
        {
            _logger.LogInformation($"{_wsHandlerType.FullName} connected");
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
            _logger.LogInformation($"{_wsHandlerType.FullName} disconnected");
            _wsHandler.WsSessionStorage.Delete(_wsHandlerType, _wsHandler.CurrentWebSocket);

            return _wsHandler.OnDisconnectedAsync(webSocketCloseStatus, wsCloseStatusDescription, exception);
        }
    }
}