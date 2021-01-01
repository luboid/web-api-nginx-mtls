using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WebApplicationWithNGINX
{
    public static class Program
    {
        private static void TrustCert(X509Store store, string fileName)
        {
            var fileContent = File.ReadAllText(fileName)
                    .Replace("-----BEGIN CERTIFICATE-----", "")
                    .Replace("-----END CERTIFICATE-----", "")
                    .Trim();
            var cert = new X509Certificate2(Convert.FromBase64String(fileContent));
            var search = store.Certificates
                .Cast<X509Certificate2>()
                .Where(c => c.Thumbprint.Equals(cert.Thumbprint, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (search == null)
            {
                store.Add(cert);
            }
        }

        private static void AddTrustedCA()
        {
            try
            {
                // trust root certificates here or in Dockerfile
                using var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser, OpenFlags.ReadWrite);
                store.Open(OpenFlags.ReadWrite);
                try
                {
                    foreach (var pem in Directory.EnumerateFiles("/host/ca-certs/", "*.pem"))
                    {
                        TrustCert(store, pem);
                    }
                }
                finally
                {
                    store.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void Main(string[] args)
        {
            AddTrustedCA();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
