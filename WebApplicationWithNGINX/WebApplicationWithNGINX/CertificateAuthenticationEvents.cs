using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Claims = System.Security.Claims;

namespace WebApplicationWithNGINX
{
    internal class CertificateAuthenticationEvents : Microsoft.AspNetCore.Authentication.Certificate.CertificateAuthenticationEvents
    {
        public override Task CertificateValidated(CertificateValidatedContext context)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, context.ClientCertificate.SerialNumber, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                new Claim(ClaimTypes.Name, context.ClientCertificate.Issuer, ClaimValueTypes.String, context.Options.ClaimsIssuer),
            };

            var identity = new ClaimsIdentity(claims, context.Scheme.Name);
            context.Principal = new ClaimsPrincipal(identity);
            return Task.CompletedTask;
        }

    }
}
