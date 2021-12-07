using System;
using System.Text.Json.Serialization;

#nullable enable

namespace QuickTest
{
    public partial class ErrorModel
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = default!;

        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; } = default!;

        [JsonPropertyName("trace_id")]
        public string TraceId { get; set; } = default!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = default!;
    }
}