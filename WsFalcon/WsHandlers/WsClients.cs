namespace WsFalcon.WsHandlers
{
    using System.Net.WebSockets;
    using Abstract;
    using Managers.Abstract;
    using Serializers.Abstract;
    using WsSubmitters;
    using WsSubmitters.Abstract;
    using WebSocketContext = WsFalcon.WebSocketContext;

    public class WsClients : IWsClients
    {
        private readonly IWsSessionsManager _wsSessionsManager;
        private readonly IInternalGroupManager _wsGroupManager;
        private readonly BroadcastClientSubmitter _broadcastClientSubmitter;
        private readonly WebSocketContext _webSocketContext;
        private readonly ISerializer _serializer;
        private readonly CallerSubmitter _callerSubmitter;

        public WsClients(
            IWsSessionsManager wsSessionsManager,
            IInternalGroupManager wsGroupManager,
            WebSocketContext webSocketContext,
            ISerializer serializer,
            WebSocket callerWs)
        {
            _wsSessionsManager = wsSessionsManager;
            _wsGroupManager = wsGroupManager;
            _webSocketContext = webSocketContext;
            _serializer = serializer;
            _broadcastClientSubmitter = new BroadcastClientSubmitter(wsSessionsManager, serializer, webSocketContext);
            _callerSubmitter = new CallerSubmitter(serializer, webSocketContext, callerWs);
        }

        public IWsClientSubmitter All => _broadcastClientSubmitter;

        public IWsClientSubmitter Caller => _callerSubmitter;

        public IWsClientSubmitter Group(string groupName)
        {
            return new GroupClientSubmitter(groupName, _wsSessionsManager, _wsGroupManager, _serializer, _webSocketContext);
        }
    }
}