using System.Security.Cryptography;
using System.Text;

namespace Backend.V6.Applications
{
    public class QRCodeGenerator : IQRCodeGenerator
    {
        public string GenerateQrCode(string qrStartToken, string qrStartSecret, DateTime requestStartTime)
        {
            string qrCodePrefix = "bankid";
            TimeSpan elapsedTime = DateTimeOffset.Now - requestStartTime;
            long timeInSeconds = (long)elapsedTime.TotalSeconds;
            string qrAuthCode = ComputeHmacSha256(qrStartSecret, timeInSeconds.ToString());

            string qrCode = $"{qrCodePrefix}.{qrStartToken}.{timeInSeconds}.{qrAuthCode}";
            return qrCode;
        }

        private string ComputeHmacSha256(string secret, string message)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
            }
        }
    }
}


