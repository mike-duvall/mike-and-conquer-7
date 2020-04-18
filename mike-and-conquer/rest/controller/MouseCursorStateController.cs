using System.Web.Http;

using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.rest.controller
{


    public class MouseCursorState
    {
        public string cursorState { get; set; }
    }



    public class MouseCursorStateController : ApiController
    {
        public IHttpActionResult Get()
        {
            MouseCursorState mouseCursorState = new MouseCursorState();
            mouseCursorState.cursorState = GameWorldView.instance.gameCursor.StateAsString;
            return Ok(mouseCursorState);
        }

    }
}