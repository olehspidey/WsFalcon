namespace WsFalcon.Managers
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Abstract.Generic;

    public class WsGroupManager<TWsHandler> : IInternalGroupManager<TWsHandler>
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _groups;

        public WsGroupManager()
        {
            _groups = new ConcurrentDictionary<string, HashSet<string>>();
        }

        public void AddConnectionToGroup(string connectionId, string groupName)
        {
            if (_groups.TryGetValue(groupName, out var conIds))
            {
                conIds.Add(connectionId);
            }
            else
            {
                _groups[groupName] = new HashSet<string> { connectionId };
            }
        }

        public void RemoveConnectionFromGroup(string connectionId, string groupName)
        {
            if (_groups.TryGetValue(groupName, out var conIds))
            {
                conIds.Remove(connectionId);
            }
        }

        public IReadOnlyCollection<string> GetConnectionIds(string groupName)
        {
            if (_groups.TryGetValue(groupName, out var conIds))
                return conIds;

            return new List<string>();
        }
    }
}