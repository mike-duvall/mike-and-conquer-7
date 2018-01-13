using System.Collections.Generic;
using System.Web.Http;

namespace mike_and_conquer_6
{
    public class Minigunner
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }

    }

    public class GdiMinigunnersController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return "value";
        }

        // POST mac/gidminigunners
        public IHttpActionResult Post([FromBody]string value)
        {
            int x = 3;
            Minigunner minigunner = new Minigunner();
            minigunner.id = 5;
            minigunner.x = 200;
            minigunner.y = 300;
            minigunner.health = 1000;
            return Ok(minigunner);

        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}