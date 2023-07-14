namespace Backend.V6.Models
{
    public record BankIdSignResponse
    {
        public string orderRef { get; set; } = string.Empty;
        public string autoStartToken { get; set; } = string.Empty;
        public string qrStartToken { get; set; } = string.Empty;
        public string qrStartSecret { get; set; } = string.Empty;
    }
}
