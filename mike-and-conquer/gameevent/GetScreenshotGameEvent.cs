using System.Drawing.Imaging;
using System.IO;
using mike_and_conquer;

namespace mike_and_conquer.gameevent
{
    public class GetScreenshotGameEvent : AsyncGameEvent
    {


        public GetScreenshotGameEvent()
        {
        }


        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            ScreenCapture sc = new ScreenCapture();
            MemoryStream stream = new MemoryStream();
            result = sc.CaptureScreenToMemoryStream(stream, ImageFormat.Png);
            return newGameState;
        }

        internal MemoryStream GetMemoryStream()
        {
            return (MemoryStream)GetResult();
        }
    }
}