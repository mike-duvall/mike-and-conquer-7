
using System.Web.Http;

using Point = Microsoft.Xna.Framework.Point;
using BadMinigunnerLocationException = mike_and_conquer.GameWorld.BadMinigunnerLocationException;

namespace mike_and_conquer.rest
{


    public class NodMinigunnersController : ApiController
    {

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern uint MessageBox(System.IntPtr hWnd, System.String text, System.String caption, uint type);

        public IHttpActionResult Get(int id)
        {
            Minigunner minigunner = GameWorld.instance.GetNodMinigunnerByIdViaEvent(id);
            RestMinigunner restMinigunner = new RestMinigunner();
            restMinigunner.id = minigunner.id;
            restMinigunner.x = (int)minigunner.positionInWorldCoordinates.X;
            restMinigunner.y = (int)minigunner.positionInWorldCoordinates.Y;
            restMinigunner.health = minigunner.health;
            restMinigunner.selected = minigunner.selected;
            return Ok(restMinigunner);

        }

        public IHttpActionResult Post([FromBody]RestMinigunner inputMinigunner)
        {
            try
            {
                Point minigunnerPositionInWorldCoordinates = new Point(inputMinigunner.x, inputMinigunner.y);
                Minigunner minigunner = GameWorld.instance.CreateNodMinigunnerViaEvent(minigunnerPositionInWorldCoordinates, inputMinigunner.aiIsOn);
                RestMinigunner restMinigunner = new RestMinigunner();
                restMinigunner.id = minigunner.id;
                restMinigunner.x = (int)minigunner.positionInWorldCoordinates.X;
                restMinigunner.y = (int)minigunner.positionInWorldCoordinates.Y;
                restMinigunner.health = minigunner.health;
                return Ok(restMinigunner);
            }
            catch (BadMinigunnerLocationException e)
            {
                return BadRequest("Cannot create on blocking terrain");
            }


//            Pickup with making this test pass:  def "should be able to move two separate GDI minigunners"()
//            Currently failing because it's using expected destination in screen coordinates vs minigunner in gameworld coordinates
//            Should probably convert clicked screen coordinate to actual world coordinate, and validate against that
//            Previously was passing too early due to checking against minigunner internal destinationX and Y, which is now 
//            the intermediate point it's current navigating too, vs the the end goal destinationX
//
//            new pickup:  Add Rest endpoint to take raw clicks as world coordinates rather than screen coordaintes
//            update test to use this method, then just test it arrived at specified worlf coordaintes


            catch (System.Exception e)
            {

                System.Diagnostics.Debug.WriteLine("ERROR*************************************");
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debug.WriteLine(e.StackTrace);

                // This doesn't actually properly display a message box, but it does attempt to and interrupts
                // the screen graphics, so at least it's a visual indication of an error.
                // have to look at console log to see actualy error
                // TODO:  Fix this, and make display of error dialog in game UI configurable
                MessageBox(new System.IntPtr(0), e.ToString(), "Error", 0);

                throw e;
            }
        }

    }
}