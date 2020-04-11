using System.Collections.Generic;
using System.Web.Http;
using mike_and_conquer.rest.domain;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using BadMinigunnerLocationException = mike_and_conquer.GameWorld.BadMinigunnerLocationException;

namespace mike_and_conquer.rest.controller
{

    public class GDIBarracksController : ApiController
    {
        // public IHttpActionResult Get()
        // {
        //     MCV mcv = GameWorld.instance.MCV;
        //     RestMCV restMCV = new RestMCV();
        //
        //     restMCV.x = (int)mcv.positionInWorldCoordinates.X;
        //     restMCV.y = (int)mcv.positionInWorldCoordinates.Y;
        //     // Vector2 screenPosition =
        //     //     MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(minigunner
        //     //         .positionInWorldCoordinates);
        //     // restMinigunner.screenX = (int)screenPosition.X;
        //     // restMinigunner.screenY = (int)screenPosition.Y;
        //     // restMinigunner.health = minigunner.health;
        //     // restMinigunner.selected = minigunner.selected;
        //     // restMinigunner.destinationX = (int) minigunner.destination.X;
        //     // restMinigunner.destinationY = (int)minigunner.destination.Y;
        //     return Ok(restMCV);
        //
        // }

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