using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mike_and_conquer.gamestate;
using mike_and_conquer.main;

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
