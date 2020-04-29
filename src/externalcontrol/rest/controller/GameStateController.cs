using System.Web.Http;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class RestGameState
    {
        public string gameState { get; set; }
    }
     
     
    public class GameStateController : ApiController
    {
        public IHttpActionResult Get()
        {
            GameState currentGameState = GameWorld.instance.GetCurrentGameStateViaEvent();
            RestGameState restGameState = new RestGameState();
            restGameState.gameState = currentGameState.GetName();
            return Ok(restGameState);
        }

    }
}