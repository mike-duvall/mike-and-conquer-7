using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
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

            if (GameWorldView.instance.barracksSidebarIconView != null)
            {
                sidebar.buildBarracksEnabled = true;
            }

            if (GameWorldView.instance.minigunnerSidebarIconView != null)
            {
                sidebar.buildMinigunnerEnabled = true;
            }

            return Ok(sidebar);
        }



    }
}