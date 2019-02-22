using System.Collections.Generic;
using System.Web.Http;


using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;


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
            Minigunner minigunner = GameWorld.instance.GetGDIMinigunnerByIdViaEvent(id);
            RestMinigunner restMinigunner = new RestMinigunner();
            restMinigunner.id = minigunner.id;
            restMinigunner.x = (int)minigunner.positionInWorldCoordinates.X;
            restMinigunner.y = (int)minigunner.positionInWorldCoordinates.Y;
            Vector2 screenPosition = minigunner.GetScreenPosition();
            restMinigunner.screenX = (int)screenPosition.X;
            restMinigunner.screenY = (int)screenPosition.Y;
            restMinigunner.health = minigunner.health;
            restMinigunner.selected = minigunner.selected;
            restMinigunner.destinationX = (int) minigunner.destination.X;
            restMinigunner.destinationY = (int)minigunner.destination.Y;
            return Ok(restMinigunner);
        }


        public IHttpActionResult Post([FromBody]RestMinigunner inputMinigunner)
        {
            
            Point minigunnerPositionInWorldCoordinates = new Point(inputMinigunner.x, inputMinigunner.y);
            Minigunner minigunner =
                GameWorld.instance.CreateGDIMinigunnerViaEvent(minigunnerPositionInWorldCoordinates);

            RestMinigunner restMinigunner = new RestMinigunner();
            restMinigunner.id = minigunner.id;
            restMinigunner.x = (int)minigunner.positionInWorldCoordinates.X;
            restMinigunner.y = (int)minigunner.positionInWorldCoordinates.Y;
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