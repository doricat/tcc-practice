using System.Text.Json.Serialization;

namespace ApiModels
{
    public class InnerError
    {
        public string Code { get; set; }

        [JsonPropertyName("innerError")]
        public InnerError InnerErrorObj { get; set; }
    }
}