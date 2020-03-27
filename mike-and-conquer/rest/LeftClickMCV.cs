﻿using System.Web.Http;

using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer.rest
{

    public class LeftClickMCVController : ApiController
    {


        // public IHttpActionResult Post([FromBody]RestMinigunnerId restMinigunnerId)
        // {
        //     Minigunner gdiMinigunner = GameWorld.instance.GetGdiOrNodMinigunner(restMinigunnerId.id);
        //
        //     Vector2 minigunnerLocation = new Vector2();
        //     minigunnerLocation.X = gdiMinigunner.positionInWorldCoordinates.X;
        //     minigunnerLocation.Y = gdiMinigunner.positionInWorldCoordinates.Y - 20;
        //
        //     Vector2 transformedLocation =
        //         MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(gdiMinigunner
        //             .positionInWorldCoordinates);
        //
        //     int screenWidth = MikeAndConquerGame.instance.defaultViewport.Width;
        //     int screenHeight = MikeAndConquerGame.instance.defaultViewport.Height;
        //
        //
        //     MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);
        //
        //     return Ok();
        // }

        public IHttpActionResult Post([FromBody]RestMinigunnerId restMinigunnerId)
        {
            // Minigunner gdiMinigunner = GameWorld.instance.GetGdiOrNodMinigunner(restMinigunnerId.id);

            MCV mcv = GameWorld.instance.MCV;

            Vector2 minigunnerLocation = new Vector2();
            minigunnerLocation.X = mcv.positionInWorldCoordinates.X;
            minigunnerLocation.Y = mcv.positionInWorldCoordinates.Y - 20;

            Vector2 transformedLocation =
                MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(mcv
                    .positionInWorldCoordinates);

            int screenWidth = MikeAndConquerGame.instance.defaultViewport.Width;
            int screenHeight = MikeAndConquerGame.instance.defaultViewport.Height;


            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint)transformedLocation.Y, screenWidth, screenHeight);

            return Ok();
        }

    }
}