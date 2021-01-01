using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WebApplicationWithNGINX
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(options =>
                {
                    options.RevocationMode = X509RevocationMode.NoCheck;
                    options.RevocationFlag = X509RevocationFlag.EndCertificateOnly;
                    options.ValidateCertificateUse = true;
                    options.ValidateValidityPeriod = true;
                    options.AllowedCertificateTypes = CertificateTypes.Chained;
                    options.Events = new WebApplicationWithNGINX.CertificateAuthenticationEvents();
                });

            services.AddAuthorization();
            services.AddCertificateForwarding(options =>
            {
                options.CertificateHeader = "X-ARR-ClientCert";
                options.HeaderConverter = (value) =>
                {
                    value = Uri.UnescapeDataString(value)
                    .Replace("-----BEGIN CERTIFICATE-----", "")
                    .Replace("-----END CERTIFICATE-----", "")
                    .Trim();
                    X509Certificate2 clientCertificate = null;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var certBuffer = Convert.FromBase64String(value);
                        clientCertificate = new X509Certificate2(certBuffer);
                    }

                    return clientCertificate;
                };
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            var forwardedHeadersOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                RequireHeaderSymmetry = false,
                ForwardLimit = null,
            };
            forwardedHeadersOptions.KnownNetworks.Clear();
            forwardedHeadersOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardedHeadersOptions);
            app.UseCertificateForwarding();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
