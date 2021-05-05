namespace WsFalcon.WsHandlers
{
    using Abstract;
    using Managers.Abstract;

    public class WsClients : IWsClients
    {
        private readonly IWsSessionsManager _wsSessionsManager;
        private readonly IInternalGroupManager _wsGroupManager;
        private readonly BroadcastClientSubmitter _broadcastClientSubmitter;

        public WsClients(IWsSessionsManager wsSessionsManager, IInternalGroupManager wsGroupManager)
        {
            _wsSessionsManager = wsSessionsManager;
            _wsGroupManager = wsGroupManager;
            _broadcastClientSubmitter = new BroadcastClientSubmitter(wsSessionsManager);
        }

        public IWsClientSubmitter All => _broadcastClientSubmitter;

        public IWsClientSubmitter Group(string groupName)
        {
            return new GroupClientSubmitter(groupName, _wsSessionsManager, _wsGroupManager);
        }
    }
}