using System.Collections.Generic;
using System.Web.Http;

namespace mike_and_conquer_6
{


    public class NodMinigunnersController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public IHttpActionResult Get(int id)
        {
            Minigunner minigunner = new Minigunner();
            minigunner.id = 5;
            minigunner.x = 200;
            minigunner.y = 300;
            minigunner.health = 0;
            return Ok(minigunner);

        }

        // POST api/values 
        public IHttpActionResult Post([FromBody]string value)
        {
            Minigunner minigunner = new Minigunner();
            minigunner.id = 5;
            minigunner.x = 200;
            minigunner.y = 300;
            minigunner.health = 1000;
            return Ok(minigunner);

        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}