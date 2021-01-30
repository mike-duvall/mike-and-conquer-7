
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
            // GameState newGameState = null;
            // GameOptions gameOptions = new GameOptions();
            // gameOptions.MAP_ZOOM = GameOptions.instance.MAP_ZOOM;
            // gameOptions.DRAW_SHROUD = GameOptions.instance.DRAW_SHROUD;
            // gameOptions.GAME_SPEED_DELAY_DIVISOR = GameOptions.instance.GAME_SPEED_DELAY_DIVISOR;
            // result = gameOptions;
            //
            // return newGameState;

            GameState newGameState = null;

            result = GameOptions.instance;

            return newGameState;


        }







    }
}
