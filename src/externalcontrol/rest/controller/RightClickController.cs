using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class RightClickController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = GameWorldView.instance.defaultViewport.Width;
            int screenHeight = GameWorldView.instance.defaultViewport.Height;

            MouseInputHandler.DoRightMouseClick((uint)point.x, (uint)point.y, screenWidth, screenHeight);
            return Ok();
        }

    }
}