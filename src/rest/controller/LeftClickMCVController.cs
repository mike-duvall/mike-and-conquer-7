using System.Web.Http;
using mike_and_conquer.gameobjects;
using mike_and_conquer.main;
using mike_and_conquer.rest.domain;
using MouseInputHandler = mike_and_conquer.main.MouseInputHandler;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.rest.controller
{

    public class LeftClickMCVController : ApiController
    {

        public IHttpActionResult Post([FromBody]RestMinigunnerId restMinigunnerId)
        {
            MCV mcv = GameWorld.instance.MCV;

            Vector2 minigunnerLocation = new Vector2();
            minigunnerLocation.X = mcv.positionInWorldCoordinates.X;
            minigunnerLocation.Y = mcv.positionInWorldCoordinates.Y - 20;

            Vector2 transformedLocation =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(mcv
                    .positionInWorldCoordinates);

            int screenWidth = GameWorldView.instance.defaultViewport.Width;
            int screenHeight = GameWorldView.instance.defaultViewport.Height;


            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint)transformedLocation.Y, screenWidth, screenHeight);

            return Ok();
        }

    }
}