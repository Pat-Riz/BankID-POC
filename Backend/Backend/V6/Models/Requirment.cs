namespace Backend.V6.Models
{
    public record Requirment
    {

        public bool pinCode { get; set; }
        public bool mrtd { get; set; }
        public string? cardReader { get; set; }
        public IEnumerable<string>? certificatePolicies { get; set; }
        public string? personalNumber { get; set; }
    }
}
