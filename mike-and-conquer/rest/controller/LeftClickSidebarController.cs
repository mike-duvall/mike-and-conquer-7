using System;
using System.Web.Http;
using Microsoft.Xna.Framework;
using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer.rest.controller
{

    public class LeftClickSidebarController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestSidebarItem sidebarItem)
        {
            // Minigunner gdiMinigunner = GameWorld.instance.GetGdiOrNodMinigunner(restMinigunnerId.id);
            //
            // Vector2 minigunnerLocation = new Vector2();
            // minigunnerLocation.X = gdiMinigunner.positionInWorldCoordinates.X;
            // minigunnerLocation.Y = gdiMinigunner.positionInWorldCoordinates.Y - 20;
            //
            // Vector2 transformedLocation =
            //     MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(gdiMinigunner
            //         .positionInWorldCoordinates);
            //
            // int screenWidth = MikeAndConquerGame.instance.defaultViewport.Width;
            // int screenHeight = MikeAndConquerGame.instance.defaultViewport.Height;
            //
            //
            // MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);






            // Point position = MikeAndConquerGame.instance.barracksToolbarIconView.GetPosition();
            // position.X = position.X + MikeAndConquerGame.instance.toolbarViewport.X;
            //
            // Vector2 positionInWorldCoordinates = new Vector2(position.X, position.Y);
            //
            // Vector2 transformedLocation =
            //     MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(positionInWorldCoordinates);
            //
            // int screenWidth = MikeAndConquerGame.instance.defaultViewport.Width;
            // int screenHeight = MikeAndConquerGame.instance.defaultViewport.Height;
            //
            //
            // MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);



            return Ok();
        }

    }
}