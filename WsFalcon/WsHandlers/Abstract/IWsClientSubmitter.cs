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
    }
}