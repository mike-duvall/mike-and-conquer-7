
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer.gameevent
{
    public class CreateNodMinigunnerGameEvent : AsyncGameEvent
    {

        private Point position;
        private bool aiIsOn;

        public CreateNodMinigunnerGameEvent(Point positionInWorldCoordinates, bool aiIsOn)
        {
            this.position = positionInWorldCoordinates;
            this.aiIsOn = aiIsOn;
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner)GetResult();
        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConquerGame.instance.AddNodMinigunner(position, aiIsOn);
            return newGameState;
        }

    }
}
