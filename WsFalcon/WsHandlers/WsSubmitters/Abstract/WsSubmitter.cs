namespace WsFalcon.WsHandlers.WsSubmitters.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Serializers.Abstract;
    using WebSocketContext = WebSocketContext;

    public abstract class WsSubmitter : IWsClientSubmitter
    {
        private readonly ISerializer _serializer;
        private readonly WebSocketContext _webSocketContext;

        protected WsSubmitter(ISerializer serializer, WebSocketContext webSocketContext)
        {
            _serializer = serializer;
            _webSocketContext = webSocketContext;
        }

        protected abstract IEnumerable<WebSocket> WebSockets { get; }

        public Task SendAsync(
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
            => Task.WhenAll(WebSockets
                .Select(ws => ws.SendAsync(bytes, messageType, endOfMessage, cancellationToken)));

        public Task SendAsync(
            string utf8String,
            CancellationToken cancellationToken = default)
            => SendAsync(
                Encoding.UTF8.GetBytes(utf8String),
                true,
                WebSocketMessageType.Text,
                cancellationToken);

        public Task SendDataAsync<TData>(
            TData data,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
            => SendAsync(
                _serializer.Serialize(data, _webSocketContext),
                true,
                WebSocketMessageType.Text,
                cancellationToken);
    }
}