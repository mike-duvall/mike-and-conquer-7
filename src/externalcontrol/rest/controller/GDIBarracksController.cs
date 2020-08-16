using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class GDIBarracksController : ApiController
    {

        public IHttpActionResult Get()
        {
            GDIBarracks gdiBarracks = GameWorld.instance.GDIBarracks;

            if (gdiBarracks == null)
            {
                return NotFound();
            }


            RestGDIBarracks  restGDIBarracks = new RestGDIBarracks();

            restGDIBarracks.x = (int)gdiBarracks.GameWorldLocation.WorldCoordinatesAsVector2.X;
            restGDIBarracks.y = (int)gdiBarracks.GameWorldLocation.WorldCoordinatesAsVector2.Y;
            return Ok(restGDIBarracks);

        }



    }
}