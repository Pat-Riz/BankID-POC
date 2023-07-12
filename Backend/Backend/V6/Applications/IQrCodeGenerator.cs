namespace Backend.V6.Applications
{
    public interface IQRCodeGenerator
    {
        string GenerateQrCode(string qrStartToken, string qrStartSecret, DateTime requestStartTime);
    }
}
