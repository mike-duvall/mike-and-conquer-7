using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gameevent 
{
    public class GetGDIMinigunnerByIdGameEvent : AsyncGameEvent
    {


        private int id;

        public GetGDIMinigunnerByIdGameEvent(int id)
        {
            this.id = id;
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner)GetResult();
        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConquerGame.instance.GetGdiMinigunner(id);
            return newGameState;
        }







    }
}
