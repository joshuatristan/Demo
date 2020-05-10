using DukeInventorySysem.Models;
using DukeInventorySysem.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DukeInventorySysem.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {

            using(var context = new DatabaseContext())
            {
                var newUser = new User();
                newUser.UserID = "2342343242";
                newUser.UserName = "Joshua";
                newUser.Password = "asdasd";
                newUser.EmailAddress = "asdasdasd";
                newUser.FirstName = "asdasdad";
                newUser.LastName = "sdfsddssddddddd";
                context.Users.Add(newUser);
                context.SaveChanges();
            }
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}