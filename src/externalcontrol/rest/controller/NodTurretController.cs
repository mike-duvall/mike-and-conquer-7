using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class NodTurretController : ApiController
    {

        public IHttpActionResult Post([FromBody]RestNodTurret inputNodTurret)
        {
            NodTurret nodTurret =
                GameWorld.instance.CreateNodTurretViaEvent(inputNodTurret.x, inputNodTurret.y, inputNodTurret.type);
        
            RestNodTurret restNodTurret = new RestNodTurret();
            restNodTurret.x = nodTurret.MapTileLocation.WorldCoordinatesAsPoint.X;
            restNodTurret.y = nodTurret.MapTileLocation.WorldCoordinatesAsPoint.Y;
            restNodTurret.type = (int)nodTurret.TurretType;
        
            return Ok(restNodTurret);
        }


    }
}