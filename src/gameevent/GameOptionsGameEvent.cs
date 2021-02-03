
using mike_and_conquer.gamestate;
using mike_and_conquer.main;


namespace mike_and_conquer.gameevent 
{
    public class GameOptionsGameEvent : AsyncGameEvent
    {




        public GameOptions GetGameOptions()
        {
            return (GameOptions)GetResult();
        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = GameOptions.instance;
            return newGameState;
        }







    }
}
