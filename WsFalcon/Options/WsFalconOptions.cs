namespace WsFalcon.Options
{
    using System.Text.Json;

    public class WsFalconOptions
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; set; }

        public int? WsBufferSize { get; set; }
    }
}