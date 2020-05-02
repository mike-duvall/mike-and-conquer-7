using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.externalcontrol.rest.controller
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

            int screenWidth = GameWorldView.instance.ScreenWidth;
            int screenHeight = GameWorldView.instance.ScreenHeight;


            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint)transformedLocation.Y, screenWidth, screenHeight);

            return Ok();
        }

    }
}