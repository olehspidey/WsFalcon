namespace WsFalcon.Serializers
{
    using System;
    using Abstract;

    public class JsonSerializer : ISerializer
    {
        public ArraySegment<byte> Serialize<TData>(TData data, WebSocketContext wsContext)
            => System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data);
    }
}