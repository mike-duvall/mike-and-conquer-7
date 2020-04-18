
using System.Web.Http;
using mike_and_conquer.gameobjects;
using mike_and_conquer.main;
using mike_and_conquer.rest.domain;

namespace mike_and_conquer.rest.controller
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

            restGdiConstructionYard.x = (int)gdiConstructionYard.positionInWorldCoordinates.X;
            restGdiConstructionYard.y = (int)gdiConstructionYard.positionInWorldCoordinates.Y;
            return Ok(restGdiConstructionYard);

        }


    }
}