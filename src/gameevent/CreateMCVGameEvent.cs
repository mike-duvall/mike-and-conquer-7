
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamestate;
using mike_and_conquer.main;
using Point = Microsoft.Xna.Framework.Point;


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
