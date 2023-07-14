using System.Text.Json.Serialization;

namespace Backend.V6.Models
{
    public record AuthRequest
    {
        public string endUserIp { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Requirment? requirment { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? userVisibleData { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? userNonVisibleData { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? userVisibleDataFormat { get; set; }
    }
}
