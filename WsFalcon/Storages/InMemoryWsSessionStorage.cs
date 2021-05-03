namespace WsFalcon.Storages
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net.WebSockets;
    using Abstract;

    public class InMemoryWsSessionStorage : IWsSessionStorage
    {
        private readonly ConcurrentDictionary<Type, HashSet<WebSocket>> _webSockets;

        public InMemoryWsSessionStorage()
        {
            _webSockets = new ConcurrentDictionary<Type, HashSet<WebSocket>>();
        }

        public void SaveWebSocket(Type wsHandlerType, WebSocket webSocket)
        {
            if (_webSockets.TryGetValue(wsHandlerType, out var wss))
            {
                wss.Add(webSocket);
            }
            else
            {
                _webSockets[wsHandlerType] = new HashSet<WebSocket> { webSocket };
            }
        }

        public void Delete(Type wsHandlerType, WebSocket webSocket)
        {
            if (_webSockets.TryGetValue(wsHandlerType, out var wss))
            {
                if (wss.TryGetValue(webSocket, out var wsToDelete))
                {
                    wss.Remove(wsToDelete);
                }
            }
        }

        public IReadOnlyCollection<WebSocket> GetWebSockets(Type wsHandlerType)
            => _webSockets.TryGetValue(wsHandlerType, out var wss)
                ? wss
                : new HashSet<WebSocket>();
    }
}