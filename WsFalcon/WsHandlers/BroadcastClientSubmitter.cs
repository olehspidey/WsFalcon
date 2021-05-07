namespace WsFalcon.WsHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using Abstract;
    using Managers.Abstract;
    using Serializers.Abstract;
    using WebSocketContext = WebSocketContext;

    public class BroadcastClientSubmitter : WsSubmitter
    {
        private readonly IWsSessionsManager _wsSessionsManager;

        public BroadcastClientSubmitter(IWsSessionsManager wsSessionsManager, ISerializer serializer, WebSocketContext webSocketContext)
            : base(serializer, webSocketContext)
        {
            _wsSessionsManager = wsSessionsManager;
        }

        protected override IEnumerable<WebSocket> WebSockets => _wsSessionsManager
            .GetWebSocketSessions()
            .Select(wss => wss.WebSocket);
    }
}