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
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, WsSession>> _wsSessions;
        private readonly Type _handlerType;

        public WsSessionsManager()
        {
            _wsSessions = new ConcurrentDictionary<Type, ConcurrentDictionary<string, WsSession>>();
            _handlerType = typeof(TWsHandler);
        }

        public void SaveWebSocketSession(WsSession wsSession)
        {
            if (_wsSessions.TryGetValue(_handlerType, out var wss))
            {
                wss[wsSession.ConnectionId] = wsSession;
            }
            else
            {
                _wsSessions[_handlerType] = new ConcurrentDictionary<string, WsSession>
                {
                    [wsSession.ConnectionId] = wsSession
                };
            }
        }

        public void Delete(WsSession wsSession)
        {
            if (_wsSessions.TryGetValue(_handlerType, out var wss))
            {
                wss.TryRemove(wsSession.ConnectionId, out _);
            }
        }

        public IReadOnlyCollection<WsSession> GetWebSocketSessions()
            => _wsSessions.TryGetValue(_handlerType, out var wss)
                ? wss.Values as ReadOnlyCollection<WsSession> ?? wss.Values.ToList().AsReadOnly()
                : new HashSet<WsSession>();

        public IReadOnlyCollection<WsSession> GetWebSocketSessions(IReadOnlyCollection<string> connectionIds)
        {
            var result = new List<WsSession>(connectionIds.Count);

            if (_wsSessions.TryGetValue(_handlerType, out var wss))
            {
                foreach (var connectionId in connectionIds)
                {
                    if (wss.TryGetValue(connectionId, out var wSession))
                        result.Add(wSession);
                }
            }

            return result;
        }
    }
}