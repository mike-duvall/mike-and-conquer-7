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
            result = GameWorld.instance.GetNodMinigunner(id);
            return newGameState;


        }







    }
}
