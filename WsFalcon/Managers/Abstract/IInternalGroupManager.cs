namespace WsFalcon.Managers.Abstract
{
    using System.Collections.Generic;

    public interface IInternalGroupManager : IGroupManager
    {
        IReadOnlyCollection<string> GetConnectionIds(string groupName);

        void RemoveConnectionFromAllGroups(string connectionId);
    }
}