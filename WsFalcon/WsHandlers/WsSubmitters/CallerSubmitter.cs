namespace WsFalcon.WsHandlers.WsSubmitters
{
    using System.Collections.Generic;
    using System.Net.WebSockets;
    using Abstract;
    using Serializers.Abstract;
    using WebSocketContext = WsFalcon.WebSocketContext;

    public class CallerSubmitter : WsSubmitterBase
    {
        private readonly WebSocket _callerWs;

        public CallerSubmitter(ISerializer serializer, WebSocketContext webSocketContext, WebSocket callerWs)
            : base(serializer, webSocketContext)
        {
            _callerWs = callerWs;
        }

        protected override IEnumerable<WebSocket> WebSockets => new[] { _callerWs };
    }
}