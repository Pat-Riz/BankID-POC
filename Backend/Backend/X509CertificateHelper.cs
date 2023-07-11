using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Common
{
    public static class X509CertificateHelper
    {
        
        public static X509Certificate2 GetCertificate(IConfiguration configuration)
        {
            var serial = configuration.GetSection("ServiceOptions")["BankIDCertSerial"];
            var fileName = configuration.GetSection("ServiceOptions")["BankIDCertPath"];

            if (!string.IsNullOrEmpty(serial))
            {
                return GetCertificateFromStore(serial);
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                var password = configuration.GetSection("ServiceOptions")["BankIDCertPassword"] 
                    ?? throw new Exception("Certificate password is obligatory");

                return ReadCertificateFromFile(fileName, password);
            }

            throw new Exception("Certificate is not configured");
        }

        private static X509Certificate2 GetCertificateFromStore(string certificateSerialNumber)
        {
            //Clean the serialnumber string.
            var rgx = new Regex("[^a-fA-F0-9]");
            var serialnumber = rgx.Replace(certificateSerialNumber, string.Empty).ToUpper();

            X509Certificate2 certToUse = null;
            X509Store computerCaStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            computerCaStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificatesInStore = computerCaStore.Certificates;

            X509Certificate2Collection findResult = certificatesInStore.Find(X509FindType.FindBySerialNumber, serialnumber, false);

            if (findResult.Count != 1)
            {
                throw new ArgumentException("Certificate was not found on server!");
            }

            foreach (X509Certificate2 foundCert in findResult)
            {
                certToUse = foundCert;
            }

            return certToUse;
        }

        private static X509Certificate2 ReadCertificateFromFile(string fileName, string password)
        {
            return new X509Certificate2(fileName, password);
        }
    }
}
