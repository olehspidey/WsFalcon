namespace WsFalcon.WsHandlers.Abstract
{
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IWsClientSubmitter
    {
        Task SendAsync(
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default);

        Task SendAsync(
            string utf8String,
            CancellationToken cancellationToken = default);

        Task SendDataAsync<TData>(
            TData data,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default);
    }
}