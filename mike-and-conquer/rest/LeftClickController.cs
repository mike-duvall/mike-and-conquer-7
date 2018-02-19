
using System.Web.Http;





using WindowsInput.Native;
using WindowsInput;

namespace mike_and_conquer.rest
{


    public class LeftClickController : ApiController
    {


        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        ////Mouse actions
        //private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        //private const int MOUSEEVENTF_LEFTUP = 0x04;
        //private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        //private const int MOUSEEVENTF_RIGHTUP = 0x10;


        //public void DoMouseClick(uint mouseX, uint mouseY)
        //{
        //    //Call the imported function with the cursor's current position
        //    //uint X = (uint)Cursor.Position.X;
        //    //uint Y = (uint)Cursor.Position.Y;
        //    //mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        //    mouse_event(MOUSEEVENTF_LEFTDOWN , mouseX, mouseY, 0, 0);
        //    mouse_event(MOUSEEVENTF_LEFTUP, mouseX, mouseY, 0, 0);
        //}

        public void DoMouseClick(uint mouseX, uint mouseY)
        {
            InputSimulator sim = new InputSimulator();

            sim.Mouse.MoveMouseTo(mouseX, mouseY);
            sim.Mouse.Sleep(1000);
            sim.Mouse.MoveMouseTo(mouseX + 200, mouseY + 200);
            sim.Mouse.LeftButtonClick();

            Pickup here using ::SendInput like in old C++ version
        }


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
//            MikeAndConqueryGame.instance.HandleLeftClick(point.x, point.y);
            DoMouseClick((uint)point.x, (uint)point.y);
            return Ok();

        }

    }
}