namespace WsFalcon.Managers
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Abstract;

    public class WsGroupManager : IGroupManager
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
                _groups[groupName] = new HashSet<string> { groupName };
            }
        }

        public void RemoveConnectionFromGroup(string connectionId, string groupName)
        {
            if (_groups.TryGetValue(groupName, out var conIds))
            {
                conIds.Remove(connectionId);
            }
        }
    }
}