using Backend.V6.Models;

public record CollectResponse
{

    public string qrCode { get; set; } = string.Empty;
    public string status { get; set; } = string.Empty;
    public string? hintCode { get; set; }
    public CompletionData? completionData { get; set; }
}