
using System.Web.Http;
using Microsoft.Xna.Framework;
using mike_and_conquer.rest.domain;
using MouseInputHandler = mike_and_conquer_6.mike_and_conquer.MouseInputHandler;

using Vector2 = Microsoft.Xna.Framework.Vector2;

using GameWorldView = mike_and_conquer.gameview.GameWorldView;


namespace mike_and_conquer.rest.controller
{

    public class LeftClickSidebarController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestSidebarItem sidebarItem)
        {
            Point position = new Point();

            if (sidebarItem.item == "Barracks")
            {
                position = GameWorldView.instance.barracksSidebarIconView.GetPosition();
            }
            else if (sidebarItem.item == "Minigunner")
            {
                position = GameWorldView.instance.minigunnerSidebarIconView.GetPosition();
            }

            Vector2 positionInWorldCoordinates = new Vector2(position.X, position.Y);

            Vector2 transformedLocation =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinatesForSidebar(positionInWorldCoordinates);


            int screenWidth = GameWorldView.instance.defaultViewport.Width;
            int screenHeight = GameWorldView.instance.defaultViewport.Height;
           
            
            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);

            return Ok();
        }

    }
}