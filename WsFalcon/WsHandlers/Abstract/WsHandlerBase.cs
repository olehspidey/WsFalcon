namespace WsFalcon.WsHandlers.Abstract
{
    using System;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Serializers.Abstract;
    using Storages.Abstract;
    using WebSocketContext = WebSocketContext;

    public abstract class WsHandlerBase
    {
        public WebSocketContext WsContext { get; internal set; } = null!;

        internal IWsSessionStorage WsSessionStorage { get; set; } = null!;

        internal WebSocket CurrentWebSocket { get; set; } = null!;

        internal ISerializer Serializer { get; set; } = null!;

        public Task SendAsync(
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
            => SendAsync(CurrentWebSocket, bytes, endOfMessage, messageType, cancellationToken);

        public Task SendUtf8Async(
            string utf8TextMessage,
            bool endOfMessage = true,
            CancellationToken cancellationToken = default)
            => SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(utf8TextMessage)), endOfMessage, WebSocketMessageType.Text, cancellationToken);

        public Task SendAsync<TData>(
            TData data,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Text,
            CancellationToken cancellationToken = default)
            => SendAsync(
                CurrentWebSocket,
                Serializer.Serialize(data, WsContext),
                endOfMessage,
                messageType,
                cancellationToken);

        public Task BroadcastAsync(
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
        {
            var sendTasks = WsSessionStorage
                .GetWebSockets(GetType())
                .Select(ws => SendAsync(ws, bytes, endOfMessage, messageType, cancellationToken));

            return Task.WhenAll(sendTasks);
        }

        public Task BroadcastUtf8Async(
            string utf8TextMessage,
            bool endOfMessage = true,
            CancellationToken cancellationToken = default)
        {
            var sendTasks = WsSessionStorage
                .GetWebSockets(GetType())
                .Select(ws => SendAsync(
                    ws,
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(utf8TextMessage)),
                    endOfMessage,
                    WebSocketMessageType.Text,
                    cancellationToken));

            return Task.WhenAll(sendTasks);
        }

        public Task BroadcastAsync<TData>(
            TData data,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Text,
            CancellationToken cancellationToken = default)
        {
            var sendTasks = WsSessionStorage
                .GetWebSockets(GetType())
                .Select(ws => SendAsync(ws, Serializer.Serialize(data, WsContext), endOfMessage, messageType, cancellationToken));

            return Task.WhenAll(sendTasks);
        }

        public virtual Task OnConnectedAsync()
            => Task.CompletedTask;

        public virtual Task OnDisconnectedAsync(
            WebSocketCloseStatus? webSocketCloseStatus,
            string wsCloseStatusDescription,
            Exception? exception)
            => Task.CompletedTask;

        public abstract Task OnMessageAsync(ArraySegment<byte> message);

        private static Task SendAsync(
            WebSocket webSocket,
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
            => webSocket.SendAsync(bytes, messageType, endOfMessage, cancellationToken);
    }
}