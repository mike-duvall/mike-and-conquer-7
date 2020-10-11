using mike_and_conquer.gameobjects;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.gameevent 
{
    public class GetNodTurretByIdGameEvent : AsyncGameEvent
    {


        private int id;

        public GetNodTurretByIdGameEvent(int id)
        {
            this.id = id;
        }

        public NodTurret GetNodTurret() { 
            return (NodTurret)GetResult();
        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = GameWorld.instance.GetNodTurret(id);
            return newGameState;
        }



    }
}
