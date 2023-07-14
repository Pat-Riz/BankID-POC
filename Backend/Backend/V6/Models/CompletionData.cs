using Models;

namespace Backend.V6.Models
{
    public class CompletionData
    {
        public User User { get; set; }
        public Device Device { get; set; }
        public Cert Cert { get; set; }
        public string Signature { get; set; }
        public string OcspResponse { get; set; }
    }
}