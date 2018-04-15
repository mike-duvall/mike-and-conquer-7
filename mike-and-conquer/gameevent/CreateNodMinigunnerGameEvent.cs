using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gameevent 
{
    public class CreateGDIMinigunnerGameEvent : AsyncGameEvent
    {


        private int x, y;

        public CreateGDIMinigunnerGameEvent(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner)GetResult();
        }



        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConqueryGame.instance.AddGdiMinigunner(x,y);
            return newGameState;
        }







    }
}
