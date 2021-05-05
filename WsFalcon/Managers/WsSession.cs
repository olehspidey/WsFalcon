namespace WsFalcon.Managers
{
    using System.Net.WebSockets;

    public class WsSession
    {
        public WsSession(string connectionId, WebSocket webSocket)
        {
            ConnectionId = connectionId;
            WebSocket = webSocket;
        }

        public string ConnectionId { get; }

        public WebSocket WebSocket { get; }
    }
}