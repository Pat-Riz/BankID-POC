namespace Backend.V6.Models
{
    public record AuthResponse
    {
        public string qrCode { get; set; }
        public string orderRef { get; set; }
    }
}