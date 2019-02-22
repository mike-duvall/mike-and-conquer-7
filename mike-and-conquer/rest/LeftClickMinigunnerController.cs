using System.Web.Http;

using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer.rest
{

    public class LeftClickMinigunnerController : ApiController
    {

        public IHttpActionResult Post([FromBody]RestMinigunnerId restMinigunnerId)
        {
            //            Minigunner gdiMinigunner = MikeAndConquerGame.instance.GetGdiOrNodMinigunner(restMinigunnerId.id);
            Minigunner gdiMinigunner = GameWorld.instance.GetGdiOrNodMinigunner(restMinigunnerId.id);
        
            Vector2 minigunnerLocation = new Microsoft.Xna.Framework.Vector2();
            minigunnerLocation.X = gdiMinigunner.positionInWorldCoordinates.X;
            minigunnerLocation.Y = gdiMinigunner.positionInWorldCoordinates.Y;
        
            Vector2 transformedLocation = gdiMinigunner.GetScreenPosition();
        
            int screenWidth = MikeAndConquerGame.instance.GraphicsDevice.Viewport.Width;
            int screenHeight = MikeAndConquerGame.instance.GraphicsDevice.Viewport.Height;

            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);

            return Ok();
        }

    }
}