using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gameevent 
{
    public class CreateNodMinigunnerGameEvent : AsyncGameEvent
    {


        private int x, y;
        private bool aiIsOn;

        public CreateNodMinigunnerGameEvent(int x, int y, bool aiIsOn)
        {
            this.x = x;
            this.y = y;
            this.aiIsOn = aiIsOn;
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner)GetResult();
        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConqueryGame.instance.AddNodMinigunner(x,y, aiIsOn);
            return newGameState;
        }

    }
}
