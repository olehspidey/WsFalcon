namespace WsFalcon.Managers.Abstract
{
    public interface IGroupManager
    {
        void AddConnectionToGroup(string connectionId, string groupName);

        void RemoveConnectionFromGroup(string connectionId, string groupName);
    }
}