using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;
using Vector2 = Microsoft.Xna.Framework.Vector2;

using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class LeftClickMinigunnerController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestMinigunnerId restMinigunnerId)
        {
            Minigunner gdiMinigunner = GameWorld.instance.GetGdiOrNodMinigunner(restMinigunnerId.id);
        
            Vector2 minigunnerLocation = new Vector2();
            minigunnerLocation.X = gdiMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X;
            minigunnerLocation.Y = gdiMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y - 20;

            Vector2 transformedLocation =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(gdiMinigunner
                    .GameWorldLocation.WorldCoordinatesAsVector2);

            int screenWidth = GameWorldView.instance.ScreenWidth;
            int screenHeight = GameWorldView.instance.ScreenHeight;


            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);

            return Ok();
        }

    }
}