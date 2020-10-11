using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class NodTurretController : ApiController
    {

        public IHttpActionResult Post([FromBody]RestNodTurret inputNodTurret)
        {
            NodTurret nodTurret =
                GameWorld.instance.CreateNodTurretViaEvent(inputNodTurret.x, inputNodTurret.y, inputNodTurret.direction, inputNodTurret.type);
        
            RestNodTurret restNodTurret = new RestNodTurret();
            restNodTurret.id = nodTurret.id;
            restNodTurret.x = nodTurret.MapTileLocation.WorldCoordinatesAsPoint.X;
            restNodTurret.y = nodTurret.MapTileLocation.WorldCoordinatesAsPoint.Y;
            restNodTurret.direction = nodTurret.Direction;
            restNodTurret.type = (int)nodTurret.TurretType;
        
            return Ok(restNodTurret);
        }


        public IHttpActionResult Get(int id)
        {
            NodTurret nodTurret = GameWorld.instance.GetNodTurretByIdViaEvent(id);
            RestNodTurret restNodTurret = new RestNodTurret();
            restNodTurret.id = nodTurret.id;
            restNodTurret.x = (int) nodTurret.MapTileLocation.WorldCoordinatesAsVector2.X;
            restNodTurret.y = (int)nodTurret.MapTileLocation.WorldCoordinatesAsVector2.Y;
            restNodTurret.direction = nodTurret.Direction;
            restNodTurret.type = nodTurret.TurretType;
            return Ok(restNodTurret);
        }



    }
}