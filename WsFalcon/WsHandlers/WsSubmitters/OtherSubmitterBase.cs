namespace WsFalcon.WsHandlers.WsSubmitters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using Abstract;
    using Managers.Abstract;
    using Serializers.Abstract;
    using WebSocketContext = WebSocketContext;

    public class OtherSubmitterBase : WsSubmitterBase
    {
        private readonly IWsSessionsManager _wsSessionsManager;
        private readonly string _currentConId;

        public OtherSubmitterBase(ISerializer serializer, WebSocketContext webSocketContext, IWsSessionsManager wsSessionsManager)
            : base(serializer, webSocketContext)
        {
            _wsSessionsManager = wsSessionsManager;
            _currentConId = webSocketContext.ConnectionInfo.Id;
        }

        protected override IEnumerable<WebSocket> WebSockets => _wsSessionsManager
            .GetWebSocketSessionsInstead(session => session.ConnectionId == _currentConId)
            .Select(wss => wss.WebSocket);
    }
}