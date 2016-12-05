using System.IdentityModel.Selectors;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;
using Thinktecture.IdentityModel.Owin;

[assembly: OwinStartup(typeof(ClientCertificates.Web.Startup))]

namespace ClientCertificates.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            var options = new ClientCertificateAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                Validator = X509CertificateValidator.PeerOrChainTrust
            };
            app.UseClientCertificateAuthentication(options);
        }
    }
}
