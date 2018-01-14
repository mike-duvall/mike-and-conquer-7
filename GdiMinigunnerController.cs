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

        public IHttpActionResult Get(int id)
        {
            Minigunner minigunner = new Minigunner();
            minigunner.id = 5;
            minigunner.x = minigunnerX;
            minigunner.y = minigunnerY;
            minigunner.health = 1000;
            return Ok(minigunner);

        }

        static int minigunnerX = -1;
        static int minigunnerY = -1;

        Make POST add minigunner to Game object instead of this hard coded static shit

        public IHttpActionResult Post([FromBody]Minigunner inputMinigunner)
        {

            Minigunner minigunner = new Minigunner();
            minigunner.id = 5;
            minigunner.x = inputMinigunner.x;
            minigunner.y = inputMinigunner.y;
            minigunner.health = 1000;
            minigunnerX = minigunner.x;
            minigunnerY = minigunner.y;
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