
using System.Runtime.InteropServices;
using System.Web.Http;


using IntPtr = System.IntPtr;


namespace mike_and_conquer.rest
{



    public class LeftClickController : ApiController
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        struct INPUT
        {
            public int type;
            public MOUSEKEYBDHARDWAREINPUT mkhi;
        }



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


        private const int MOUSEEVENTF_MOVE = 0x01;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick(uint mouseX, uint mouseY)
        {


            double fScreenWidth = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Width;
            double fScreenHeight = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Height; 
            double fx = mouseX * (65535.0f / fScreenWidth);
            double fy = mouseY * (65535.0f / fScreenHeight);


            INPUT x = new INPUT();
            x.type = 0;
            //x.mkhi.mi.dx = 564;
            //x.mkhi.mi.dy = 425;
            //x.mkhi.mi.dx = (int)mouseX;
            //x.mkhi.mi.dy = (int)mouseY;
            x.mkhi.mi.dx = (int)fx;
            x.mkhi.mi.dy = (int)fy;


            x.mkhi.mi.mouseData = 0;
            //x.mkhi.mi.dwFlags = 0x01;
            x.mkhi.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            x.mkhi.mi.time = 1000;
//            x.mkhi.mi.dwExtraInfo = GetMessageExtraInfo();
            int size = Marshal.SizeOf(x);

            uint y3 = SendInput(1, ref x, size);


            x.mkhi.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
            uint y = SendInput(1, ref x, size);

            System.Threading.Thread.Sleep(1000);

            x.mkhi.mi.dwFlags = MOUSEEVENTF_LEFTUP;
            uint y2 = SendInput(1, ref x, size);




            //        mouseInput.mi.dwFlags = MOUSEEVENTF_LEFTDOWN; ;

            //        ::SendInput(1, &mouseInput, sizeof(INPUT));

            //        std::this_thread::sleep_for(std::chrono::milliseconds(1000));
            //        mouseInput.mi.dwFlags = MOUSEEVENTF_LEFTUP;

            //::SendInput(1, &mouseInput, sizeof(INPUT));


        }


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
//            MikeAndConqueryGame.instance.HandleLeftClick(point.x, point.y);
            DoMouseClick((uint)point.x, (uint)point.y);
            return Ok();

        }

    }
}