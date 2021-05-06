namespace WsFalcon
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Http;

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Properties will be used for getting info")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Properties should be public for getting info")]
    public class WebSocketContext
    {
        public WebSocketContext(ConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;
        }

        public ConnectionInfo ConnectionInfo { get; }
    }
}