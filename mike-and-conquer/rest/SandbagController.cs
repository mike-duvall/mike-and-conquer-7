using System.Collections.Generic;
using System.Web.Http;


using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace mike_and_conquer.rest
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