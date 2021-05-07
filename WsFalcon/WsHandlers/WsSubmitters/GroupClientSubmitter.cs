namespace WsFalcon.WsHandlers.WsSubmitters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using Abstract;
    using Managers.Abstract;
    using Serializers.Abstract;
    using WebSocketContext = WebSocketContext;

    public class GroupClientSubmitter : WsSubmitterBase
    {
        private readonly string _groupName;
        private readonly IWsSessionsManager _wsSessionsManager;
        private readonly IInternalGroupManager _groupManager;

        public GroupClientSubmitter(
            string groupName,
            IWsSessionsManager wsSessionsManager,
            IInternalGroupManager groupManager,
            ISerializer serializer,
            WebSocketContext webSocketContext)
            : base(serializer, webSocketContext)
        {
            _groupName = groupName;
            _wsSessionsManager = wsSessionsManager;
            _groupManager = groupManager;
        }

        protected override IEnumerable<WebSocket> WebSockets => _wsSessionsManager
            .GetWebSocketSessions(_groupManager
                .GetConnectionIds(_groupName))
            .Select(wss => wss.WebSocket);
    }
}