using System.Collections.Generic;
using System.Web.Http;


using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace mike_and_conquer.rest
{

    public class SandbagController : ApiController
    {
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

//        public IHttpActionResult Get(int id)
//        {
//            Minigunner minigunner = MikeAndConquerGame.instance.GetGDIMinigunnerByIdViaEvent(id);
//            RestMinigunner restMinigunner = new RestMinigunner();
//            restMinigunner.id = minigunner.id;
//            restMinigunner.x = (int)minigunner.position.X;
//            restMinigunner.y = (int)minigunner.position.Y;
//            Vector2 screenPosition = minigunner.GetScreenPosition();
//            restMinigunner.screenX = (int)screenPosition.X;
//            restMinigunner.screenY = (int)screenPosition.Y;
//            restMinigunner.health = minigunner.health;
//            restMinigunner.selected = minigunner.selected;
//            restMinigunner.destinationX = (int) minigunner.destination.X;
//            restMinigunner.destinationY = (int)minigunner.destination.Y;
//            return Ok(restMinigunner);
//        }


        public IHttpActionResult Post([FromBody]RestSandbag inputSandbag)
        {
//            Minigunner minigunner = MikeAndConquerGame.instance.CreateGDIMinigunnerViaEvent(inputMinigunner.x, inputMinigunner.y);
//
//            RestMinigunner restMinigunner = new RestMinigunner();
//            restMinigunner.id = minigunner.id;
//            restMinigunner.x = (int)minigunner.position.X;
//            restMinigunner.y = (int)minigunner.position.Y;
//            restMinigunner.health = minigunner.health;

            Sandbag sandbag =
                MikeAndConquerGame.instance.CreateSandbagViaEvent(inputSandbag.x, inputSandbag.y, inputSandbag.index);

            RestSandbag restSandbag = new RestSandbag();
            restSandbag.x = (int)sandbag.position.X;
            restSandbag.y = (int)sandbag.position.Y;
            restSandbag.index = (int) sandbag.SandbagType;

            return Ok(sandbag);
        }

//        public void Put(int id, [FromBody]string value)
//        {
//        }
//
//        public void Delete(int id)
//        {
//        }
    }
}