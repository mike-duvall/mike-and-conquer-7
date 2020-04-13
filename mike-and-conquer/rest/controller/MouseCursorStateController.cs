using System.Web.Http;

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
            mouseCursorState.cursorState = MikeAndConquerGame.instance.gameCursor.StateAsString;
            return Ok(mouseCursorState);
        }

    }
}