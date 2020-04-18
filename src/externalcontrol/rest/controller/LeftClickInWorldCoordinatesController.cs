using System.Web.Http;
using Microsoft.Xna.Framework;
using mike_and_conquer.externalcontrol.rest.domain;
using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class LeftClickInWorldCoordinatesController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = GameWorldView.instance.defaultViewport.Width;
            int screenHeight = GameWorldView.instance.defaultViewport.Height;

            Vector2 locationInWorldCoordinates = new Vector2(point.x, point.y);
            Vector2 locationInScreenCoordinates = GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(locationInWorldCoordinates);
            MouseInputHandler.DoLeftMouseClick((uint)locationInScreenCoordinates.X, (uint)locationInScreenCoordinates.Y, screenWidth, screenHeight);
            return Ok();
        }

    }
}