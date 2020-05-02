using System.Web.Http;
using Microsoft.Xna.Framework;
using mike_and_conquer.externalcontrol.rest.domain;
using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class LeftClickAndHoldInWorldCoordinatesController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = GameWorldView.instance.ScreenWidth;
            int screenHeight = GameWorldView.instance.ScreenHeight;

            Vector2 locationInWorldCoordinates = new Vector2(point.x, point.y);
            Vector2 locationInScreenCoordinates = GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(locationInWorldCoordinates);
            MouseInputHandler.DoLeftMouseClickAndHold((uint)locationInScreenCoordinates.X, (uint)locationInScreenCoordinates.Y, screenWidth, screenHeight);
            return Ok();
        }

    }
}