using System.Collections.Generic;
using System.Web.Http;

namespace mike_and_conquer.rest
{

    public class RestGameState
    {
        public string gameState { get; set; }
    }


    public class GameStateController : ApiController
    {
        public IHttpActionResult Get()
        {
            GameState currentGameState = MikeAndConqueryGame.instance.GetCurrentGameStateViaEvent();
            RestGameState restGameState = new RestGameState();
            restGameState.gameState = currentGameState.GetName();
            return Ok(restGameState);
        }

    }
}