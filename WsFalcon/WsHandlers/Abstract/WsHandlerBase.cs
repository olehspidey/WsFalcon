namespace WsFalcon.WsHandlers.Abstract
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using Managers.Abstract;
    using Serializers.Abstract;
    using WebSocketContext = WebSocketContext;

    /// <summary>
    /// Represents base implementation of web socket handler.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Properties will be used in implementation class")]
    public abstract class WsHandlerBase
    {
        /// <summary>
        /// Gets web socket context.
        /// </summary>
        public WebSocketContext WsContext { get; internal set; } = null!;

        /// <summary>
        /// Gets or sets data serializer. If it is not configured, will be used default json serializer.
        /// </summary>
        public ISerializer Serializer { get; set; } = null!;

        /// <summary>
        /// Gets group manager.
        /// </summary>
        public IGroupManager Group { get; internal set; } = null!;

        /// <summary>
        /// Gets an object that can be used to invoke methods on the clients connected to this hub.
        /// </summary>
        public IWsClients Clients { get; internal set; } = null!;

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
        /// <param name="endOfMessage">Indicates whether the message has been received completely.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task OnMessageAsync(ArraySegment<byte> message, bool endOfMessage)
            => Task.CompletedTask;
    }
}