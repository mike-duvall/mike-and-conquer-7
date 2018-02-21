
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


        private const int MOUSEEVENTF_MOVE = 0x01;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseClick(uint mouseX, uint mouseY)
        {
            int screenWidth = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Width;
            int screenHeight = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Height;

            // Must normalize mouse coordinates.
            // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms646260(v=vs.85).aspx
            double normalizedMouseX = mouseX * (65535.0f / screenWidth);
            double normalizedMouseY = mouseY * (65535.0f / screenHeight);

            INPUT mouseInput = new INPUT();
            mouseInput.type = 0;
            mouseInput.mkhi.mi.mouseData = 0;
            mouseInput.mkhi.mi.time = 1000;
            int size = Marshal.SizeOf(mouseInput);
            mouseInput.mkhi.mi.dx = (int)normalizedMouseX;
            mouseInput.mkhi.mi.dy = (int)normalizedMouseY;


            mouseInput.mkhi.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            uint y3 = SendInput(1, ref mouseInput, size);

            mouseInput.mkhi.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
            uint y = SendInput(1, ref mouseInput, size);

            System.Threading.Thread.Sleep(1000);

            mouseInput.mkhi.mi.dwFlags = MOUSEEVENTF_LEFTUP;
            uint y2 = SendInput(1, ref mouseInput, size);
        }


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            DoMouseClick((uint)point.x, (uint)point.y);
            return Ok();
        }

    }
}