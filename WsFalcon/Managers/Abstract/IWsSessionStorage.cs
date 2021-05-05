namespace WsFalcon.Managers.Abstract
{
    using System;
    using System.Collections.Generic;

    public interface IWsSessionStorage
    {
        void SaveWebSocketSession(Type wsHandlerType, WsSession wsSession);

        IReadOnlyCollection<WsSession> GetWebSocketSessions(Type wsHandlerType);

        void Delete(Type wsHandlerType, WsSession wsSession);
    }
}