
using Point = Microsoft.Xna.Framework.Point;

using Exception = System.Exception;

using BadMinigunnerLocationException = mike_and_conquer.GameWorld.BadMinigunnerLocationException;

namespace mike_and_conquer.gameevent
{
    public class CreateMCVGameEvent : AsyncGameEvent
    {

        private Point position;

        public CreateMCVGameEvent(Point positionInWorldCoordinates)
        {
            this.position = positionInWorldCoordinates;
        }

        public MCV GetMCV()
        {
            return (MCV)GetResult();

        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConquerGame.instance.AddMCVAtWorldCoordinates(position);
            return newGameState;
        }

    }
}
