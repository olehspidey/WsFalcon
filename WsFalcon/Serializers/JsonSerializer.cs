namespace WsFalcon.Serializers
{
    using System;
    using Abstract;

    public class JsonSerializer : ISerializer
    {
        public ArraySegment<byte> Serialize<TData>(TData data)
            => System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data);
    }
}