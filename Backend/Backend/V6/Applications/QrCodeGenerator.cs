using System.Security.Cryptography;
using System.Text;

namespace Backend.V6.Applications
{
    public record QRCodeData(string OrderRef, string StartToken, string StartSecret, DateTime RequestStartTime);

    public class QRCodeGenerator : IQRCodeGenerator
    {
        private readonly Dictionary<string, QRCodeData> _qrCodes;

        public QRCodeGenerator()
        {
            _qrCodes = new Dictionary<string, QRCodeData>();
        }

        public string GenerateQrCode(string orderRef, string qrStartToken, string qrStartSecret, DateTime requestStartTime)
        {
            string qrCode = GenerateQrCode(qrStartToken, qrStartSecret, requestStartTime);
            _qrCodes.Add(orderRef, new QRCodeData(orderRef, qrStartToken, qrStartSecret, requestStartTime));

            return qrCode;
        }

        private string GenerateQrCode(string qrStartToken, string qrStartSecret, DateTime requestStartTime)
        {
            string qrCodePrefix = "bankid";
            TimeSpan elapsedTime = DateTimeOffset.Now - requestStartTime;
            long timeInSeconds = (long)elapsedTime.TotalSeconds;
            string qrAuthCode = ComputeHmacSha256(qrStartSecret, timeInSeconds.ToString());

            return $"{qrCodePrefix}.{qrStartToken}.{timeInSeconds}.{qrAuthCode}";
        }

        public string UpdateQRCode(string orderRef)
        {
            string qrCode = "";
            if (_qrCodes.ContainsKey(orderRef))
            {
                var qrCodeData = _qrCodes[orderRef];
                qrCode = GenerateQrCode(qrCodeData.StartToken, qrCodeData.StartSecret, qrCodeData.RequestStartTime);
            }
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

        public void RemoveQRCode(string orderRef)
        {
            _qrCodes.Remove(orderRef);
            //TODO: Also remove all data with RequestStartTime Older than 1 hour/day? Incase of "skräpdata"
        }
    }
}


