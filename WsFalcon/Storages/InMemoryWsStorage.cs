namespace WsFalcon.Storages
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net.WebSockets;
    using Abstract;

    public class InMemoryWsStorage : IWsStorage
    {
        private readonly ConcurrentDictionary<Type, List<WebSocket>> _webSockets;

        public InMemoryWsStorage()
        {
            _webSockets = new ConcurrentDictionary<Type, List<WebSocket>>();
        }

        public void SaveWs(Type wsHandlerType, WebSocket webSocket)
        {
            if (_webSockets.TryGetValue(wsHandlerType, out var wss))
            {
                wss.Add(webSocket);
            }
            else
            {
                _webSockets[wsHandlerType] = new List<WebSocket> { webSocket };
            }
        }

        public IReadOnlyCollection<WebSocket> GetWebSockets(Type wsHandlerType)
            => _webSockets.TryGetValue(wsHandlerType, out var wss)
                ? wss
                : new List<WebSocket>();
    }
}