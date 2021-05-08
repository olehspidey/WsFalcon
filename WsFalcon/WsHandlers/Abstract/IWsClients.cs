namespace WsFalcon.WsHandlers.Abstract
{
    using WsSubmitters.Abstract;

    public interface IWsClients
    {
        /// <summary>
        /// Gets a <see cref="IWsClientSubmitter"/> that can be used to invoke methods on all clients connected to the ws handler.
        /// </summary>
        IWsClientSubmitter All { get; }

        /// <summary>
        /// Gets a caller to the connection which triggered the current invocation.
        /// </summary>
        IWsClientSubmitter Caller { get; }

        /// <summary>
        /// Gets a caller to all connections except the one which triggered the current invocation.
        /// </summary>
        IWsClientSubmitter Other { get; }

        /// <summary>
        /// Gets a <see cref="IWsClientSubmitter"/> that can be used to invoke methods on all connections in the specified group.
        /// </summary>
        /// <param name="groupName">The group name.</param>
        /// <returns>Client submitter.</returns>
        IWsClientSubmitter Group(string groupName);
    }
}