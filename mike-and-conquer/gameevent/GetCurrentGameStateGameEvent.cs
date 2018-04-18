using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gameevent 
{
    public class GetCurrentGameStateGameEvent : AsyncGameEvent
    {

        public GameState GetGameState()
        {
            return (GameState)GetResult();
        }



        protected override GameState ProcessImpl()
        {
            GameState currentGameState = MikeAndConqueryGame.instance.GetCurrentGameState();
            result = currentGameState;
            return currentGameState;
        }







    }
}
