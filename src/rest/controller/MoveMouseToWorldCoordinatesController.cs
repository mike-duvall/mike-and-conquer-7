using System.Web.Http;
using Microsoft.Xna.Framework;
using mike_and_conquer.rest.domain;
using MouseInputHandler = mike_and_conquer.main.MouseInputHandler;

using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.rest.controller
{

    public class MoveMouseToWorldCoordinatesController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = GameWorldView.instance.defaultViewport.Width;
            int screenHeight = GameWorldView.instance.defaultViewport.Height;

            Vector2 locationInWorldCoordinates = new Vector2(point.x, point.y);
            Vector2 locationInScreenCoordinates = GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(locationInWorldCoordinates);
            MouseInputHandler.MoveMouseToCoordinates((uint)locationInScreenCoordinates.X, (uint)locationInScreenCoordinates.Y, screenWidth, screenHeight);
            return Ok();
        }

    }
}