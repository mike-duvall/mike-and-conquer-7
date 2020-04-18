using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamestate;
using mike_and_conquer.main;

namespace mike_and_conquer.gameevent 
{
    public class CreateSandbagGameEvent : AsyncGameEvent
    {


        private int x, y, index;

        public CreateSandbagGameEvent(int x, int y, int index)
        {
            this.x = x;
            this.y = y;
            this.index = index;
        }

        public Sandbag GetSandbag()
        {
            return (Sandbag)GetResult();
        }



        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConquerGame.instance.AddSandbag(x,y, index);
            return newGameState;
        }







    }
}
