namespace WsFalcon
{
    using Microsoft.AspNetCore.Http;

    public class WebSocketContext
    {
        public WebSocketContext(ConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;
        }

        public ConnectionInfo ConnectionInfo { get; }
    }
}