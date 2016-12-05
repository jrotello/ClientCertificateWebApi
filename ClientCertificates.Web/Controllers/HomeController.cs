using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Mvc;
using Thinktecture.IdentityModel;

namespace ClientCertificates.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public async Task<ActionResult> ClientCertificate()
        {
            var handler = new WebRequestHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
            };

            var thumbprint = ConfigurationManager.AppSettings["ClientCertThumbprint"];
            var cert = X509.CurrentUser.My.Thumbprint.Find(thumbprint).First();
            handler.ClientCertificates.Add(cert);
            handler.ServerCertificateValidationCallback = ServerCertificateValidationCallback;

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"{Request?.Url?.Scheme}://{Request?.Url?.Authority}")
            };

            var resp = await client.GetAsync("/api/claims");
            if (!resp.IsSuccessStatusCode)
            {
                return Content(resp.ReasonPhrase);
            }

            return Content(await resp.Content.ReadAsStringAsync());
        }

        private bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
