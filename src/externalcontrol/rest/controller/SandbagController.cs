using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class SandbagController : ApiController
    {

        public IHttpActionResult Post([FromBody]RestSandbag inputSandbag)
        {
            Sandbag sandbag =
                GameWorld.instance.CreateSandbagViaEvent(inputSandbag.x, inputSandbag.y, inputSandbag.index);

            RestSandbag restSandbag = new RestSandbag();
            restSandbag.x = sandbag.MapTileLocation.WorldCoordinatesAsPoint.X;
            restSandbag.y = sandbag.MapTileLocation.WorldCoordinatesAsPoint.Y;
            restSandbag.index = (int) sandbag.SandbagType;

            return Ok(sandbag);
        }

    }
}