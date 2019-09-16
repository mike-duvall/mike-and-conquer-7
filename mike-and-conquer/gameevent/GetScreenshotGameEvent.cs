

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            result = MikeAndConquerGame.instance.SaveScreenshot();
            return newGameState;
        }

        internal MemoryStream GetMemoryStream()
        {
            return (MemoryStream)GetResult();
        }
    }
}
