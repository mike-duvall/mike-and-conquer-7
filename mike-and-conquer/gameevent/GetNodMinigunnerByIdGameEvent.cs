using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gameevent 
{
    public class GetNodMinigunnerByIdGameEvent : AsyncGameEvent
    {


        private int id;

        public GetNodMinigunnerByIdGameEvent(int id)
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
            result = MikeAndConqueryGame.instance.GetNodMinigunner(id);
            return newGameState;


        }







    }
}
