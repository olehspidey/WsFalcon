namespace WsFalcon.Managers.Abstract
{
    using System.Collections.Generic;

    public interface IWsSessionsManager
    {
        void SaveWebSocketSession(WsSession wsSession);

        IReadOnlyCollection<WsSession> GetWebSocketSessions();

        IReadOnlyCollection<WsSession> GetWebSocketSessions(IReadOnlyCollection<string> connectionIds);

        void Delete(WsSession wsSession);
    }
}