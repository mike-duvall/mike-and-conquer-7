using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mike_and_conquer.gamestate;
using mike_and_conquer.main;

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
            GameState currentGameState = MikeAndConquerGame.instance.GetCurrentGameState();
            result = currentGameState;
            return currentGameState;
        }

    }
}
