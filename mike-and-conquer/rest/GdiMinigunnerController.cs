using System.Collections.Generic;
using System.Web.Http;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using BadMinigunnerLocationException = mike_and_conquer.GameWorld.BadMinigunnerLocationException;

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
            Vector2 screenPosition =
                MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(minigunner
                    .positionInWorldCoordinates);
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

            MikeAndConquerGame.instance.log.Information("Entering Post() for minigunner");

            Point minigunnerPositionInWorldCoordinates = new Point(inputMinigunner.x, inputMinigunner.y);

            Minigunner minigunner;

            try
            {
                minigunner =
                    GameWorld.instance.CreateGDIMinigunnerViaEvent(minigunnerPositionInWorldCoordinates);
            }
            catch (BadMinigunnerLocationException e)
            {
                return BadRequest("Cannot create on blocking terrain");
            }

            RestMinigunner restMinigunner = new RestMinigunner();
            restMinigunner.id = minigunner.id;
            restMinigunner.x = (int)minigunner.positionInWorldCoordinates.X;
            restMinigunner.y = (int)minigunner.positionInWorldCoordinates.Y;
            restMinigunner.health = minigunner.health;

            // MikeAndConquerGame.instance.log.Information("leaving Post() for minigunner, id:" + restMinigunner.id);
            return Ok(restMinigunner);
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
            GameWorld.instance.SetGDIMinigunnerHealthViaEvent(id, 0);
        }
    }
}