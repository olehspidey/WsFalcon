namespace WsFalcon.Managers.Abstract
{
    using System;
    using System.Collections.Generic;

    public interface IWsSessionsManager
    {
        void SaveWebSocketSession(WsSession wsSession);

        IReadOnlyCollection<WsSession> GetWebSocketSessions();

        IReadOnlyCollection<WsSession> GetWebSocketSessions(IReadOnlyCollection<string> connectionIds);

        IReadOnlyCollection<WsSession> GetWebSocketSessionsInstead(Func<WsSession, bool> predicate);

        void Delete(WsSession wsSession);
    }
}