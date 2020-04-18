
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamestate;
using mike_and_conquer.main;
using Point = Microsoft.Xna.Framework.Point;

using Exception = System.Exception;

using BadMinigunnerLocationException = mike_and_conquer.main.GameWorld.BadMinigunnerLocationException;

namespace mike_and_conquer.gameevent
{
    public class CreateGDIMinigunnerGameEvent : AsyncGameEvent
    {

        private Point position;

        public CreateGDIMinigunnerGameEvent(Point positionInWorldCoordinates)
        {
            this.position = positionInWorldCoordinates;
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner)GetResult();

        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConquerGame.instance.AddGdiMinigunner(position);
            return newGameState;
        }

    }
}
