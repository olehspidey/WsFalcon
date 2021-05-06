namespace WsFalcon.Managers
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Abstract.Generic;

    public class WsGroupManager<TWsHandler> : IInternalGroupManager<TWsHandler>
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _groupToConIds;
        private readonly ConcurrentDictionary<string, HashSet<string>> _conIdToGroups;

        public WsGroupManager()
        {
            _groupToConIds = new ConcurrentDictionary<string, HashSet<string>>();
            _conIdToGroups = new ConcurrentDictionary<string, HashSet<string>>();
        }

        public void AddConnectionToGroup(string connectionId, string groupName)
        {
            if (_groupToConIds.TryGetValue(groupName, out var conIds))
            {
                conIds.Add(connectionId);

                if (_conIdToGroups.TryGetValue(connectionId, out var groups))
                {
                    groups.Add(groupName);
                }
                else
                {
                    _conIdToGroups[connectionId] = new HashSet<string> { groupName };
                }
            }
            else
            {
                _groupToConIds[groupName] = new HashSet<string> { connectionId };

                if (_conIdToGroups.TryGetValue(connectionId, out var groups))
                {
                    groups.Add(groupName);
                }
                else
                {
                    _conIdToGroups[connectionId] = new HashSet<string> { groupName };
                }
            }
        }

        public void RemoveConnectionFromGroup(string connectionId, string groupName)
        {
            if (_groupToConIds.TryGetValue(groupName, out var conIds))
            {
                conIds.Remove(connectionId);
                _conIdToGroups[connectionId].Remove(groupName);
            }
        }

        public void RemoveConnectionFromAllGroups(string connectionId)
        {
            if (_conIdToGroups.TryGetValue(connectionId, out var groups))
            {
                foreach (var group in groups)
                {
                    _groupToConIds[group].Remove(connectionId);
                }

                _conIdToGroups.TryRemove(connectionId, out _);
            }
        }

        public IReadOnlyCollection<string> GetConnectionIds(string groupName)
        {
            if (_groupToConIds.TryGetValue(groupName, out var conIds))
                return conIds;

            return new List<string>();
        }
    }
}