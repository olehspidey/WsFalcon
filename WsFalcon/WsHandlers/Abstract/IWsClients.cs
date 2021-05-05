namespace WsFalcon.WsHandlers.Abstract
{
    public interface IWsClients
    {
        IWsClientSubmitter All { get; }

        IWsClientSubmitter Group(string groupName);
    }
}