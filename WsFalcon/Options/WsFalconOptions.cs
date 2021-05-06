namespace WsFalcon.Options
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Properties can be seted up in Startup")]
    public class WsFalconOptions
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; set; }

        public int? WsBufferSize { get; set; }
    }
}