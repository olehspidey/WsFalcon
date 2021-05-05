namespace WsFalcon.Managers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;

    public class InMemoryWsSessionStorage : IWsSessionStorage
    {
        private readonly ConcurrentDictionary<Type, IDictionary<string, WsSession>> _wsSessions;

        public InMemoryWsSessionStorage()
        {
            _wsSessions = new ConcurrentDictionary<Type, IDictionary<string, WsSession>>();
        }

        public void SaveWebSocketSession(Type wsHandlerType, WsSession wsSession)
        {
            if (_wsSessions.TryGetValue(wsHandlerType, out var wss))
            {
                wss[wsSession.ConnectionId] = wsSession;
            }
            else
            {
                _wsSessions[wsHandlerType] = new Dictionary<string, WsSession>
                {
                    [wsSession.ConnectionId] = wsSession
                };
            }
        }

        public void Delete(Type wsHandlerType, WsSession wsSession)
        {
            if (_wsSessions.TryGetValue(wsHandlerType, out var wss))
            {
                wss.Remove(wsSession.ConnectionId);
            }
        }

        public IReadOnlyCollection<WsSession> GetWebSocketSessions(Type wsHandlerType)
            => _wsSessions.TryGetValue(wsHandlerType, out var wss)
                ? wss.Values.ToList()
                : new HashSet<WsSession>();
    }
}