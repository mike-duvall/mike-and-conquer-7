using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class GDIConstructionYardController : ApiController
    {

        public IHttpActionResult Get()
        {
            GDIConstructionYard gdiConstructionYard = GameWorld.instance.GDIConstructionYard;

            if (gdiConstructionYard == null)
            {
                return NotFound();
            }
            RestGDIConstructionYard  restGdiConstructionYard = new RestGDIConstructionYard();

            restGdiConstructionYard.x = gdiConstructionYard.MapTileLocation.WorldCoordinatesAsPoint.X;
            restGdiConstructionYard.y = gdiConstructionYard.MapTileLocation.WorldCoordinatesAsPoint.Y;
            return Ok(restGdiConstructionYard);

        }


    }
}