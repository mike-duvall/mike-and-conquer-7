using System.Collections.Generic;
using System.Web.Http;

namespace mike_and_conquer.rest
{


    public class LeftClickController : ApiController
    {

        public IHttpActionResult Post([FromBody]string value)
        {
            int x = 3;
            RestMinigunner minigunner = new RestMinigunner();
            minigunner.id = 5;
            minigunner.x = 200;
            minigunner.y = 300;
            minigunner.health = 1000;
            return Ok(minigunner);

        }

    }
}