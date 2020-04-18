using System.Web.Http;
using mike_and_conquer.gameobjects;
using mike_and_conquer.main;
using mike_and_conquer.rest.domain;


namespace mike_and_conquer.rest.controller
{

    public class SandbagController : ApiController
    {

        public IHttpActionResult Post([FromBody]RestSandbag inputSandbag)
        {
            Sandbag sandbag =
                GameWorld.instance.CreateSandbagViaEvent(inputSandbag.x, inputSandbag.y, inputSandbag.index);

            RestSandbag restSandbag = new RestSandbag();
            restSandbag.x = (int)sandbag.positionInWorldCoordinates.X;
            restSandbag.y = (int)sandbag.positionInWorldCoordinates.Y;
            restSandbag.index = (int) sandbag.SandbagType;

            return Ok(sandbag);
        }

    }
}