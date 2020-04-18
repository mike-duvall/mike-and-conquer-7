
using System.Web.Http;
using mike_and_conquer.gameobjects;
using mike_and_conquer.main;
using mike_and_conquer.rest.domain;

namespace mike_and_conquer.rest.controller
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

            restGDIBarracks.x = (int)gdiBarracks.positionInWorldCoordinates.X;
            restGDIBarracks.y = (int)gdiBarracks.positionInWorldCoordinates.Y;
            return Ok(restGDIBarracks);

        }



    }
}