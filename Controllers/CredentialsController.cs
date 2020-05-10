using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DukeInventorySysem.Controllers
{
    public class CredentialsController : ApiController
    {
        // GET: api/Credentials
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Credentials/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Credentials
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Credentials/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Credentials/5
        public void Delete(int id)
        {
        }
    }
}
