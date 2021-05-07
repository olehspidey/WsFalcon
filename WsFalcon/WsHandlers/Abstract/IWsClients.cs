namespace WsFalcon.WsHandlers.Abstract
{
    using WsSubmitters.Abstract;

    public interface IWsClients
    {
        IWsClientSubmitter All { get; }

        IWsClientSubmitter Group(string groupName);
    }
}