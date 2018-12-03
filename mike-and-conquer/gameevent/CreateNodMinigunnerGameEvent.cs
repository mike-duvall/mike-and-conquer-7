
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer.gameevent 
{
    public class CreateGDIMinigunnerGameEvent : AsyncGameEvent
    {


        private int x, y;
        private Point position;

        public CreateGDIMinigunnerGameEvent(Point worldCoordinates )
        {
            this.position = worldCoordinates;
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner)GetResult();
        }



        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConquerGame.instance.AddGdiMinigunner(this.position);
            return newGameState;
        }







    }
}
