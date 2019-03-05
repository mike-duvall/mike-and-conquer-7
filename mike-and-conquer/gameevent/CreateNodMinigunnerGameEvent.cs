
using Point = Microsoft.Xna.Framework.Point;

using Exception = System.Exception;

using BadMinigunnerLocationException = mike_and_conquer.GameWorld.BadMinigunnerLocationException;

namespace mike_and_conquer.gameevent 
{
    public class CreateGDIMinigunnerGameEvent : AsyncGameEvent
    {



        private Point position;
        private Exception thrownException;

        public CreateGDIMinigunnerGameEvent(Point positionInWorldCoordinates)
        {
            this.position = positionInWorldCoordinates;
            this.thrownException = null;
        }

        public Minigunner GetMinigunner()
        {
            Minigunner minigunner = (Minigunner)GetResult();
            if (thrownException != null)
            {
                throw thrownException;
            }
            else
            {
                return minigunner;
            }

        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            try
            {
                result = MikeAndConquerGame.instance.AddGdiMinigunner(this.position);
            }
            catch (BadMinigunnerLocationException e)
            {
                thrownException = e;
            }

            return newGameState;
        }

    }
}
