using System.Collections.Generic;
using System.Web.Http;

namespace mike_and_conquer_6
{

    public class GameState
    {
        public string gameState { get; set; }
    }


    public class GameStateController : ApiController
    {
        public IHttpActionResult Get()
        {
            GameState gameState = new GameState();
            gameState.gameState = "Game Over";
            return Ok(gameState);
        }

    }
}