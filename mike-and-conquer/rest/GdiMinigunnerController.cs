using System.Collections.Generic;
using System.Web.Http;

namespace mike_and_conquer.rest
{

    public class GdiMinigunnersController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public IHttpActionResult Get(int id)
        {
            RestMinigunner minigunner = new RestMinigunner();
            minigunner.id = 5;
            minigunner.x = minigunnerX;
            minigunner.y = minigunnerY;
            minigunner.health = 1000;
            return Ok(minigunner);

        }

        static int minigunnerX = -1;
        static int minigunnerY = -1;

        public IHttpActionResult Post([FromBody]RestMinigunner inputMinigunner)
        {

            RestMinigunner minigunner = new RestMinigunner();
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