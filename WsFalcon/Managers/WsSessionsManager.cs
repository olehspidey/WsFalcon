namespace WsFalcon.Managers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Abstract.Generic;

    public class WsSessionsManager<TWsHandler> : IWsSessionsManager<TWsHandler>
    {
        private readonly ConcurrentDictionary<string, WsSession> _wsSessions;

        public WsSessionsManager()
        {
            _wsSessions = new ConcurrentDictionary<string, WsSession>();
        }

        public void SaveWebSocketSession(WsSession wsSession)
        {
            _wsSessions[wsSession.ConnectionId] = wsSession;
        }

        public void Delete(WsSession wsSession)
        {
            _wsSessions.TryRemove(wsSession.ConnectionId, out _);
        }

        public IReadOnlyCollection<WsSession> GetWebSocketSessions()
            => _wsSessions.Values as ReadOnlyCollection<WsSession> ?? _wsSessions.Values.ToList().AsReadOnly();

        public IReadOnlyCollection<WsSession> GetWebSocketSessions(IReadOnlyCollection<string> connectionIds)
        {
            var result = new List<WsSession>(connectionIds.Count);

            foreach (var connectionId in connectionIds)
            {
                if (_wsSessions.TryGetValue(connectionId, out var wSession))
                    result.Add(wSession);
            }

            return result;
        }

        public IReadOnlyCollection<WsSession> GetWebSocketSessionsInstead(Func<WsSession, bool> predicate)
            => _wsSessions.Values.Where(session => !predicate(session)).ToList();
    }
}