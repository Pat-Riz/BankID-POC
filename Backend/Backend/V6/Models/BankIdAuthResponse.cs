namespace Backend.V6.Models
{
    public record BankIdAuthResponse
    {
        public string orderRef { get; set; } = string.Empty;
        public string autoStartToken { get; set; } = string.Empty;
        public string qrStartToken { get; set; } = string.Empty;
        public string qrStartSecret { get; set; } = string.Empty;

    }
}
