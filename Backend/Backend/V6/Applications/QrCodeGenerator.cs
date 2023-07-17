using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace Backend.V6.Applications
{
    public record QRCodeData(string OrderRef, string StartToken, string StartSecret, DateTime RequestStartTime);

    public class QRCodeGenerator : IQRCodeGenerator
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public QRCodeGenerator(IMemoryCache cache)
        {
            _cache = cache;
            // BankID authentication is only valid for 30 secs, then it timesout. So we only want to store the values for around that long.
            _cacheOptions = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) };
        }

        public string GenerateQrCode(string orderRef, string qrStartToken, string qrStartSecret, DateTime requestStartTime)
        {
            string qrCode = GenerateQrCode(qrStartToken, qrStartSecret, requestStartTime);
            _cache.Set(orderRef, new QRCodeData(orderRef, qrStartToken, qrStartSecret, requestStartTime), _cacheOptions);

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
            if (_cache.TryGetValue<QRCodeData>(orderRef, out var result) && result is not null)
            {
                qrCode = GenerateQrCode(result.StartToken, result.StartSecret, result.RequestStartTime);
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

    }
}


