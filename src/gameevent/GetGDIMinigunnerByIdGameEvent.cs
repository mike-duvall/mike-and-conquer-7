using mike_and_conquer.gameobjects;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.gameevent 
{
    public class GetGDIMinigunnerByIdGameEvent : AsyncGameEvent
    {


        private int id;

        public GetGDIMinigunnerByIdGameEvent(int id)
        {
            this.id = id;
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner)GetResult();
        }

        protected override GameState ProcessImpl()
        {
            GameState newGameState = null;
            result = GameWorld.instance.GetGdiMinigunner(id);
            return newGameState;
        }







    }
}
