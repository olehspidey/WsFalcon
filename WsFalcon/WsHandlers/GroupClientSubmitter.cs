namespace WsFalcon.WsHandlers
{
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstract;
    using Managers.Abstract;

    public class GroupClientSubmitter : IWsClientSubmitter
    {
        private readonly string _groupName;
        private readonly IWsSessionStorage _wsSessionStorage;

        public GroupClientSubmitter(string groupName, IWsSessionStorage wsSessionStorage)
        {
            _groupName = groupName;
            _wsSessionStorage = wsSessionStorage;
        }

        public Task SendAsync(
            ArraySegment<byte> bytes,
            bool endOfMessage = true,
            WebSocketMessageType messageType = WebSocketMessageType.Binary,
            CancellationToken cancellationToken = default)
        {
        }
    }
}