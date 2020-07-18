using System.Web.Http;
using Microsoft.Xna.Framework;
using mike_and_conquer.externalcontrol.rest.domain;
using Vector2 = Microsoft.Xna.Framework.Vector2;

using GameWorldView = mike_and_conquer.gameview.GameWorldView;


namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class LeftClickSidebarController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestSidebarItem sidebarItem)
        {
            Point position = new Point();

            if (sidebarItem.item == "Barracks")
            {
                position = GameWorldView.instance.BarracksSidebarIconView.GetPosition();
            }
            else if (sidebarItem.item == "Minigunner")
            {
                position = GameWorldView.instance.MinigunnerSidebarIconView.GetPosition();
            }

            Vector2 positionInWorldCoordinates = new Vector2(position.X, position.Y);

            Vector2 transformedLocation =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinatesForSidebar(positionInWorldCoordinates);


            int screenWidth = GameWorldView.instance.ScreenWidth;
            int screenHeight = GameWorldView.instance.ScreenHeight;
           
            
            MouseInputHandler.MoveMouseToCoordinates((uint)transformedLocation.X, (uint)transformedLocation.Y, screenWidth, screenHeight);
            
            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint) transformedLocation.Y , screenWidth, screenHeight);

            return Ok();
        }

    }
}