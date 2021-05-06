namespace WsFalcon.Managers.Abstract.Generic
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "For singleton instantiation per handler")]
    public interface IGroupManager<TWsHandler> : IGroupManager
    {
    }
}