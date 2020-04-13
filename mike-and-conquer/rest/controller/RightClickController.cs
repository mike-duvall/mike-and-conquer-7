using System.Web.Http;
using mike_and_conquer.rest.domain;
using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

namespace mike_and_conquer.rest.controller
{

    public class RightClickController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = MikeAndConquerGame.instance.defaultViewport.Width;
            int screenHeight = MikeAndConquerGame.instance.defaultViewport.Height;

            MouseInputHandler.DoRightMouseClick((uint)point.x, (uint)point.y, screenWidth, screenHeight);
            return Ok();
        }

    }
}