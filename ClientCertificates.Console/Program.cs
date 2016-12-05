using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Thinktecture.IdentityModel;

namespace ClientCertificates.Console
{
    class Program
    {
        static void Main()
        {
            var thumbprint = ConfigurationManager.AppSettings["ClientCertThumbprint"];
            var cert = X509.CurrentUser.My.Thumbprint.Find(thumbprint).First();

            var handler = new WebRequestHandler {ClientCertificateOptions = ClientCertificateOption.Manual};
            handler.ClientCertificates.Add(cert);
            
            // *** NOT SECURE - DON'T DO THIS IN PRODUCTION *******************************************************
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            // ****************************************************************************************************

            var client = new HttpClient(handler);
            
            var resp = client.GetAsync("https://localhost:44352/api/claims").Result;
            if (!resp.IsSuccessStatusCode)
            {
                System.Console.WriteLine(resp.ReasonPhrase);
            }
            else
            {
                var json = JsonConvert.DeserializeObject(resp.Content.ReadAsStringAsync().Result);
                System.Console.WriteLine(json);
            }

            System.Console.ReadLine();
        }
    }
}
