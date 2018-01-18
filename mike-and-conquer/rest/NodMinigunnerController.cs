using System.Collections.Generic;
using System.Web.Http;

namespace mike_and_conquer.rest
{


    public class NodMinigunnersController : ApiController
    {

        public IHttpActionResult Get(int id)
        {
            RestMinigunner minigunner = new RestMinigunner();
            minigunner.id = 5;
            minigunner.x = 200;
            minigunner.y = 300;
            minigunner.health = 0;
            return Ok(minigunner);

        }

        public IHttpActionResult Post([FromBody]string value)
        {
            RestMinigunner minigunner = new RestMinigunner();
            minigunner.id = 5;
            minigunner.x = 200;
            minigunner.y = 300;
            minigunner.health = 1000;
            return Ok(minigunner);

        }

    }
}