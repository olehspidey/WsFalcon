namespace WsFalcon.WsHandlers
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using Abstract;
    using Managers;
    using Managers.Abstract.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Options;
    using Serializers.Abstract;
    using WebSocketContext = WebSocketContext;

    public class WsHandlerLifeTimeManger<TWsHandler>
        where TWsHandler : WsHandlerBase
    {
        private readonly TWsHandler _wsHandler;
        private readonly Type _wsHandlerType;
        private readonly ILogger<WsHandlerLifeTimeManger<TWsHandler>> _logger;
        private readonly IWsSessionsManager<TWsHandler> _wsSessionsManager;
        private readonly int _bufferSize;
        private WsSession? _wsSession;

        public WsHandlerLifeTimeManger(IServiceProvider serviceProvider)
        {
            _wsHandler = ActivatorUtilities.GetServiceOrCreateInstance<TWsHandler>(serviceProvider);
            _wsHandlerType = typeof(TWsHandler);
            _logger = serviceProvider.GetRequiredService<ILogger<WsHandlerLifeTimeManger<TWsHandler>>>();
            _bufferSize = serviceProvider
                .GetRequiredService<IOptions<WsFalconOptions>>()
                .Value
                .WsBufferSize ?? 1024 * 4;
            _wsSessionsManager = serviceProvider.GetRequiredService<IWsSessionsManager<TWsHandler>>();
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

                await _wsHandler.OnMessageAsync(arrSegment[..receiveResult.Count], receiveResult.EndOfMessage);

                while (!webSocket.CloseStatus.HasValue)
                {
                    receiveResult = await webSocket.ReceiveAsync(arrSegment, httpContext.RequestAborted);

                    if (receiveResult.CloseStatus.HasValue)
                    {
                        await OnDisconnectedAsync(webSocket.CloseStatus, webSocket.CloseStatusDescription);

                        return;
                    }

                    await _wsHandler.OnMessageAsync(arrSegment[..receiveResult.Count], receiveResult.EndOfMessage);
                }

                await OnDisconnectedAsync(webSocket.CloseStatus, webSocket.CloseStatusDescription);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Receive message error for {_wsHandlerType.FullName}");
                await OnDisconnectedAsync(webSocket.CloseStatus, webSocket.CloseStatusDescription, e);
            }
        }

        private Task OnConnectedAsync(HttpContext httpContext, WebSocket webSocket)
        {
            var groupManager = httpContext.RequestServices.GetRequiredService<IInternalGroupManager<TWsHandler>>();
            _wsSession = new WsSession(httpContext.Connection.Id, webSocket);

            _logger.LogInformation($"{_wsHandlerType.FullName} connected");
            _wsHandler.Serializer = httpContext.RequestServices.GetRequiredService<ISerializer>();
            _wsHandler.WsContext = new WebSocketContext(httpContext.Connection);
            _wsHandler.Group = groupManager;
            _wsHandler.Clients = new WsClients(
                httpContext.RequestServices.GetRequiredService<IWsSessionsManager<TWsHandler>>(),
                groupManager);
            _wsSessionsManager.SaveWebSocketSession(_wsSession);

            return _wsHandler.OnConnectedAsync();
        }

        private Task OnDisconnectedAsync(
            WebSocketCloseStatus? webSocketCloseStatus,
            string wsCloseStatusDescription,
            Exception? exception = null)
        {
            _logger.LogInformation($"{_wsHandlerType.FullName} disconnected");

            if(_wsSession != null)
                _wsSessionsManager.Delete(_wsSession);

            return _wsHandler.OnDisconnectedAsync(webSocketCloseStatus, wsCloseStatusDescription, exception);
        }
    }
}