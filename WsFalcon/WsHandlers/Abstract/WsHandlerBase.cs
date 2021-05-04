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

    /// <summary>
    /// Represents base implementation of web socket handler.
    /// </summary>
    public abstract class WsHandlerBase
    {
        /// <summary>
        /// Gets web socket context.
        /// </summary>
        public WebSocketContext WsContext { get; internal set; } = null!;

        /// <summary>
        /// Gets data serializer. If it is not configured, will be used default json serializer.
        /// </summary>
        public ISerializer Serializer { get; internal set; } = null!;

        internal IWsSessionStorage WsSessionStorage { get; set; } = null!;

        internal WebSocket CurrentWebSocket { get; set; } = null!;

        /// <summary>
        /// Sends binary message.
        /// </summary>
        /// <param name="bytes">Binary message.</param>
        /// <param name="endOfMessage">Indicates whether the data in "buffer" is the last part of a message.</param>
        /// <param name="messageType">Indicates whether the application is sending a binary or text message.</param>
        /// <param name="cancellationToken">The token that propagates the notification that operations should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task SendAsync(
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
            => SendAsync(CurrentWebSocket, bytes, endOfMessage, messageType, cancellationToken);

        /// <summary>
        /// Sends UTF-8 text message.
        /// </summary>
        /// <param name="utf8TextMessage">UTF-8 text message.</param>
        /// <param name="endOfMessage">Indicates whether the data in "buffer" is the last part of a message.</param>
        /// <param name="cancellationToken">The token that propagates the notification that operations should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task SendUtf8Async(
            string utf8TextMessage,
            bool endOfMessage = true,
            CancellationToken cancellationToken = default)
            => SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(utf8TextMessage)), endOfMessage, WebSocketMessageType.Text, cancellationToken);

        /// <summary>
        /// Sends data.
        /// </summary>
        /// <param name="data">Data to send. This data will be serialized via <see cref="ISerializer"/>.</param>
        /// <param name="endOfMessage">Indicates whether the data in "buffer" is the last part of a message.</param>
        /// <param name="messageType">Indicates whether the application is sending a binary or text message.</param>
        /// <param name="cancellationToken">The token that propagates the notification that operations should be canceled.</param>
        /// <typeparam name="TData">Type of data to send.</typeparam>
        /// <returns>The task object representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Broadcast binary message to all active connections.
        /// </summary>
        /// <param name="bytes">Binary message.</param>
        /// <param name="endOfMessage">Indicates whether the data in "buffer" is the last part of a message.</param>
        /// <param name="messageType">Indicates whether the application is sending a binary or text message.</param>
        /// <param name="cancellationToken">The token that propagates the notification that operations should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Broadcast UTF-8 text message to all active connections.
        /// </summary>
        /// <param name="utf8TextMessage">UTF-8 text message.</param>
        /// <param name="endOfMessage">Indicates whether the data in "buffer" is the last part of a message.</param>
        /// <param name="cancellationToken">The token that propagates the notification that operations should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Broadcast data message to all active connections.
        /// </summary>
        /// <param name="data">Data to send. This data will be serialized via <see cref="ISerializer"/>.</param>
        /// <param name="endOfMessage">Indicates whether the data in "buffer" is the last part of a message.</param>
        /// <param name="messageType">Indicates whether the application is sending a binary or text message.</param>
        /// <param name="cancellationToken">The token that propagates the notification that operations should be canceled.</param>
        /// <typeparam name="TData">Type of data to send.</typeparam>
        /// <returns>The task object representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Callback method that invokes when connection is established.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task OnConnectedAsync()
            => Task.CompletedTask;

        /// <summary>
        /// Callback method that invokes when connection is canceled.
        /// </summary>
        /// <param name="webSocketCloseStatus">Indicates the reason why the remote endpoint initiated the close handshake.</param>
        /// <param name="wsCloseStatusDescription">Allows the remote endpoint to describe the reason why the connection was closed.</param>
        /// <param name="exception">Describe connection cancel error.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task OnDisconnectedAsync(
            WebSocketCloseStatus? webSocketCloseStatus,
            string wsCloseStatusDescription,
            Exception? exception)
            => Task.CompletedTask;

        /// <summary>
        /// Callback method that invokes when new message is got.
        /// </summary>
        /// <param name="message">Binary message.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
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