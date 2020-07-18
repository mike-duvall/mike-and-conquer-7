using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;
using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class SidebarController : ApiController
    {

        public IHttpActionResult Get()
        {
            RestSidebar sidebar = new RestSidebar();
            sidebar.buildBarracksEnabled = false;
            sidebar.buildMinigunnerEnabled = false;
            sidebar.barracksIsBuilding = false;

            GDIConstructionYard constructionYard = GameWorld.instance.GDIConstructionYard;
            if (constructionYard != null)
            {
                sidebar.barracksIsBuilding = constructionYard.IsBuildingBarracks;
                sidebar.barracksReadyToPlace = constructionYard.IsBarracksReadyToPlace;
            }
            

            if (GameWorldView.instance.BarracksSidebarIconView != null)
            {
                sidebar.buildBarracksEnabled = true;

            }

            if (GameWorldView.instance.MinigunnerSidebarIconView != null)
            {
                sidebar.buildMinigunnerEnabled = true;
            }

            return Ok(sidebar);
        }



    }
}