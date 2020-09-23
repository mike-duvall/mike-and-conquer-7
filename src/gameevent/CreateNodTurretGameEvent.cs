using mike_and_conquer.gameobjects;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;

namespace mike_and_conquer.gameevent 
{
    public class CreateNodTurretGameEvent : AsyncGameEvent
    {


        private int x, y, type;

        public CreateNodTurretGameEvent(int x, int y, int type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public NodTurret GetNodTurret()
        {
            return (NodTurret)GetResult();
        }



        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = MikeAndConquerGame.instance.AddNodTurret(MapTileLocation.CreateFromWorldMapTileCoordinates(x, y),
                type);
            return newGameState;
        }

    }
}
