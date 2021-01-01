using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            const string uri = "https://localhost:44381";

            using var client1 = CreateHttpClient(uri, "client1.pfx");
            var str1 = await GetWeatherForecastAsync(client1).ConfigureAwait(false);
            Console.WriteLine("client1: " + str1);

            using var client2 = CreateHttpClient(uri, "client2.pfx");
            var str2 = await GetWeatherForecastAsync(client2).ConfigureAwait(false);
            Console.WriteLine("client2: " + str2);

            Console.ReadLine();
        }

        public static HttpClient CreateHttpClient(string uri, string certificatePfx)
        {
            var handler = new HttpClientHandler
            {
                UseProxy = false,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            handler.ClientCertificates.Add(new X509Certificate2(certificatePfx, "P@ssw0rd"));

            var client = new HttpClient(handler, true)
            {
                BaseAddress = new Uri(uri)
            };

            return client;
        }

        public static async Task<string> GetWeatherForecastAsync(HttpClient client)
        {
            try
            {
                using var resp = await client.GetAsync("/weatherforecast").ConfigureAwait(false);
                // resp.EnsureSuccessStatusCode();
                return await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
