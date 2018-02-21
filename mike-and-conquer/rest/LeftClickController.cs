using System.Web.Http;
using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

namespace mike_and_conquer.rest
{

    public class LeftClickController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Width;
            int screenHeight = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Height;

            MouseInputHandler.DoMouseClick((uint)point.x, (uint)point.y, screenWidth, screenHeight);
            return Ok();
        }

    }
}