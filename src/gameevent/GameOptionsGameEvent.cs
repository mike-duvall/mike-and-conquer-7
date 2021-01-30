using mike_and_conquer.gameobjects;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;


namespace mike_and_conquer.gameevent 
{
    public class GameOptionsGameEvent : AsyncGameEvent
    {




        public GameOptions2 GetGameOptions()
        {
            return (GameOptions2)GetResult();
        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            GameOptions2 gameOptions2 = new GameOptions2();
            gameOptions2.initialMapZoom = GameOptions.INITIAL_MAP_ZOOM;
            gameOptions2.drawShroud = GameOptions.DRAW_SHROUD;
            gameOptions2.gameSpeedDelayDivisor = GameOptions.GAME_SPEED_DELAY_DIVISOR;
            result = gameOptions2;

            return newGameState;


        }







    }
}
