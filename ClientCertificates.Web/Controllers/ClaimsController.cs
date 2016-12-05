using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace ClientCertificates.Web.Controllers
{
    [Authorize]
    public class ClaimsController : ApiController
    {
        // GET api/values
        public IDictionary<string, string> Get()
        {
            var user = (ClaimsIdentity) User.Identity;
            return user.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
    }
}
