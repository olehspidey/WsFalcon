namespace WsFalcon.WsHandlers
{
    using Abstract;

    public class WsClients : IWsClients
    {
        public IWsClientSubmitter All { get; }

        public IWsClientSubmitter Group(string groupName)
        {
            throw new System.NotImplementedException();
        }
    }
}