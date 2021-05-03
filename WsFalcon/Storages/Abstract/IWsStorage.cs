namespace WsFalcon.Storages.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Net.WebSockets;

    public interface IWsStorage
    {
        void SaveWs(Type wsHandlerType, WebSocket webSocket);

        IReadOnlyCollection<WebSocket> GetWebSockets(Type wsHandlerType);
    }
}