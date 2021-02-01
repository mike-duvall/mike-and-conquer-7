
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
            // gameOptions.MapZoomLevel = GameOptions.instance.MapZoomLevel;
            // gameOptions.DrawShroud = GameOptions.instance.DrawShroud;
            // gameOptions.GameSpeedDelayDivisor = GameOptions.instance.GameSpeedDelayDivisor;
            // result = gameOptions;
            //
            // return newGameState;

            GameState newGameState = null;

            result = GameOptions.instance;

            return newGameState;


        }







    }
}
