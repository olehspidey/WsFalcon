namespace WsFalcon.Serializers
{
    using System;
    using Abstract;
    using Microsoft.Extensions.Options;
    using Options;

    public class JsonSerializer : ISerializer
    {
        private readonly WsFalconOptions _wsFalconOptions;

        public JsonSerializer(IOptions<WsFalconOptions> wsFalconOptions)
        {
            _wsFalconOptions = wsFalconOptions.Value;
        }

        public ArraySegment<byte> Serialize<TData>(TData data, WebSocketContext wsContext)
            => System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data, _wsFalconOptions.JsonSerializerOptions);
    }
}