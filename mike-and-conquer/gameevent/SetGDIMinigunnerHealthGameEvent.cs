
using Point = Microsoft.Xna.Framework.Point;

using Exception = System.Exception;

using BadMinigunnerLocationException = mike_and_conquer.GameWorld.BadMinigunnerLocationException;

namespace mike_and_conquer.gameevent
{
    public class SetGDIMinigunnerHealthGameEvent : AsyncGameEvent
    {

        private int minigunnerId;
        private int health;

        public SetGDIMinigunnerHealthGameEvent(int minigunnerId, int health)
        {
            this.minigunnerId = minigunnerId;
            this.health = health;
        }


        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            Minigunner minigunner = GameWorld.instance.GetGdiMinigunner(minigunnerId);
            // MikeAndConquerGame.instance.log.Information("Before setting health of minigunner, id: " + minigunnerId + ", health=" + health);
            minigunner.health = health;
            // MikeAndConquerGame.instance.log.Information("After setting health of minigunner, id: " + minigunnerId + ", health=" + health);

            return newGameState;
        }

    }
}
