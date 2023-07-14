namespace Backend.V6.Models
{
    public record BankIdCollectResponse
    {
        public string orderRef { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string? hintCode { get; set; }
        public CompletionData? completionData { get; set; }
    }
}
