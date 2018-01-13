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
        // GET api/values 
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        public IHttpActionResult Get()
        {
            GameState gameState = new GameState();
            gameState.gameState = "Game Over";
            return Ok(gameState);
        }


        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public IHttpActionResult Post([FromBody]string value)
        {
            int x = 3;
            Minigunner minigunner = new Minigunner();
            minigunner.id = 5;
            minigunner.x = 200;
            minigunner.y = 300;
            minigunner.health = 1000;
            return Ok(minigunner);

        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}