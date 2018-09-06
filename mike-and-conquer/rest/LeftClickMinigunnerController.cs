using System.Web.Http;

using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer.rest
{

    public class LeftClickMinigunnerController : ApiController
    {

        public IHttpActionResult Post([FromBody]RestMinigunnerId restMinigunnerId)
        {
            Minigunner gdiMinigunner = MikeAndConqueryGame.instance.GetGdiOrNodMinigunner(restMinigunnerId.id);

            Vector2 minigunnerLocation = new Microsoft.Xna.Framework.Vector2();
            minigunnerLocation.X = gdiMinigunner.position.X;
            minigunnerLocation.Y = gdiMinigunner.position.Y;

            Vector2 transformedLocation = gdiMinigunner.GetScreenPosition();

            int screenWidth = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Width;
            int screenHeight = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Height;

            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);

            return Ok();
        }

    }
}