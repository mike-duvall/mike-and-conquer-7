using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gameevent 
{
    public class ResetGameGameEvent : AsyncGameEvent
    {

        private bool drawShroud;

        public ResetGameGameEvent(bool drawShroud)
        {
            this.drawShroud = drawShroud;
        }

        protected override GameState ProcessImpl()
        {
            return MikeAndConquerGame.instance.HandleReset(drawShroud);
        }

    }
}
