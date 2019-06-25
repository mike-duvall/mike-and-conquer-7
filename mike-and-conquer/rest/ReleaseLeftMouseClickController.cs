using System.Web.Http;
using Microsoft.Xna.Framework;
using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

namespace mike_and_conquer.rest
{

    public class ReleaseLeftMouseClickController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = MikeAndConquerGame.instance.defaultViewport.Width;
            int screenHeight = MikeAndConquerGame.instance.defaultViewport.Height;

            Vector2 locationInWorldCoordinates = new Vector2(point.x, point.y);
            Vector2 locationInScreenCoordinates = MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(locationInWorldCoordinates);
            MouseInputHandler.DoReleaseLeftMouseClick((uint)locationInScreenCoordinates.X, (uint)locationInScreenCoordinates.Y, screenWidth, screenHeight);
            return Ok();
        }

    }
}