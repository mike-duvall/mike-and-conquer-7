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
            Minigunner minigunner = MikeAndConqueryGame.instance.GetGdiMinigunner(id);
            RestMinigunner restMinigunner = new RestMinigunner();
            restMinigunner.id = minigunner.id;
            restMinigunner.x = (int)minigunner.position.X;
            restMinigunner.y = (int)minigunner.position.Y;
            restMinigunner.health = minigunner.health;
            restMinigunner.selected = minigunner.selected;
            return Ok(restMinigunner);

        }


        public IHttpActionResult Post([FromBody]RestMinigunner inputMinigunner)
        {
            Minigunner minigunner = MikeAndConqueryGame.instance.AddGdiMinigunner(inputMinigunner.x, inputMinigunner.y);
            RestMinigunner restMinigunner = new RestMinigunner();
            restMinigunner.id = minigunner.id;
            restMinigunner.x = (int)minigunner.position.X;
            restMinigunner.y = (int)minigunner.position.Y;
            restMinigunner.health = minigunner.health;
            return Ok(restMinigunner);
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}