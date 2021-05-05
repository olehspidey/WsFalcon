namespace WsFalcon.WsHandlers
{
    using System;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstract;
    using Managers.Abstract;

    public class GroupClientSubmitter : IWsClientSubmitter
    {
        private readonly string _groupName;
        private readonly IWsSessionsManager _wsSessionsManager;
        private readonly IInternalGroupManager _groupManager;

        public GroupClientSubmitter(string groupName, IWsSessionsManager wsSessionsManager, IInternalGroupManager groupManager)
        {
            _groupName = groupName;
            _wsSessionsManager = wsSessionsManager;
            _groupManager = groupManager;
        }

        public Task SendAsync(
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
        {
            var connectionIds = _groupManager.GetConnectionIds(_groupName);
            var webSocketSessionsTasks = _wsSessionsManager
                .GetWebSocketSessions(connectionIds)
                .Select(wss => wss.WebSocket.SendAsync(bytes, messageType, endOfMessage, cancellationToken));

            return Task.WhenAll(webSocketSessionsTasks);
        }
    }
}