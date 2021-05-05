namespace WsFalcon.WsHandlers
{
    using System;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstract;
    using Managers.Abstract;

    public class BroadcastClientSubmitter : IWsClientSubmitter
    {
        private readonly IWsSessionsManager _wsSessionsManager;

        public BroadcastClientSubmitter(IWsSessionsManager wsSessionsManager)
        {
            _wsSessionsManager = wsSessionsManager;
        }

        public Task SendAsync(
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
        {
            var webSocketSessionsTasks = _wsSessionsManager
                .GetWebSocketSessions()
                .Select(session => session.WebSocket.SendAsync(
                    bytes,
                    messageType,
                    endOfMessage,
                    cancellationToken));

            return Task.WhenAll(webSocketSessionsTasks);
        }
    }
}