namespace Backend.V6.Applications
{
    public interface IQRCodeGenerator
    {
        string GenerateQrCode(string orderRef, string qrStartToken, string qrStartSecret, DateTime requestStartTime);
        string UpdateQRCode(string orderRef);
    }
}
