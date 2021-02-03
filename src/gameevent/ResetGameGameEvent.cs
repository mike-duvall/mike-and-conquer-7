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
        private float initialMapZoom;
        private int gameSpeedDelayDivisor;

        public ResetGameGameEvent(bool drawShroud, float initialMapZoom, int gameSpeedDelayDivisor)
        {
            this.drawShroud = drawShroud;
            this.initialMapZoom = initialMapZoom;
            this.gameSpeedDelayDivisor = gameSpeedDelayDivisor;
        }

        protected override GameState ProcessImpl()
        {
            return MikeAndConquerGame.instance.HandleReset(drawShroud, initialMapZoom, gameSpeedDelayDivisor);
        }

    }
}
