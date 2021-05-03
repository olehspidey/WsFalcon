namespace WsFalcon.Storages.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Net.WebSockets;

    public interface IWsSessionStorage
    {
        void SaveWebSocket(Type wsHandlerType, WebSocket webSocket);

        IReadOnlyCollection<WebSocket> GetWebSockets(Type wsHandlerType);

        void Delete(Type wsHandlerType, WebSocket webSocket);
    }
}