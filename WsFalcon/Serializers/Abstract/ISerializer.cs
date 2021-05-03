namespace WsFalcon.Serializers.Abstract
{
    using System;

    public interface ISerializer
    {
        ArraySegment<byte> Serialize<TData>(TData data);
    }
}