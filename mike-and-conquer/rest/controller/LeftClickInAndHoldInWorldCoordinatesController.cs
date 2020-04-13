using System.Web.Http;
using Microsoft.Xna.Framework;
using mike_and_conquer.rest.domain;
using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

namespace mike_and_conquer.rest.controller
{

    public class LeftClickAndHoldInWorldCoordinatesController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = MikeAndConquerGame.instance.defaultViewport.Width;
            int screenHeight = MikeAndConquerGame.instance.defaultViewport.Height;

            Vector2 locationInWorldCoordinates = new Vector2(point.x, point.y);
            Vector2 locationInScreenCoordinates = MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(locationInWorldCoordinates);
            MouseInputHandler.DoLeftMouseClickAndHold((uint)locationInScreenCoordinates.X, (uint)locationInScreenCoordinates.Y, screenWidth, screenHeight);
            return Ok();
        }

    }
}