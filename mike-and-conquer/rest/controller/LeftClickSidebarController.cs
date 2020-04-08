using System;
using System.Web.Http;
using Microsoft.Xna.Framework;
using mike_and_conquer.rest.domain;
using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace mike_and_conquer.rest.controller
{

    public class LeftClickSidebarController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestSidebarItem sidebarItem)
        {




            Point position = MikeAndConquerGame.instance.barracksToolbarIconView.GetPosition();
            position.X = position.X + MikeAndConquerGame.instance.toolbarViewport.X;
            
            Vector2 positionInWorldCoordinates = new Vector2(position.X, position.Y);
            
            Vector2 transformedLocation =
                MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(positionInWorldCoordinates);
            
            int screenWidth = MikeAndConquerGame.instance.defaultViewport.Width;
            int screenHeight = MikeAndConquerGame.instance.defaultViewport.Height;
            
            
            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);



            return Ok();
        }

    }
}