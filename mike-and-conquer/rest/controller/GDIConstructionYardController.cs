using System.Collections.Generic;
using System.Web.Http;
using mike_and_conquer.rest.domain;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using BadMinigunnerLocationException = mike_and_conquer.GameWorld.BadMinigunnerLocationException;

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