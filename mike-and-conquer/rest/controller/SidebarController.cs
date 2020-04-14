
using System.Web.Http;
using mike_and_conquer.rest.domain;

namespace mike_and_conquer.rest.controller
{

    public class SidebarController : ApiController
    {

        public IHttpActionResult Get()
        {
            RestSidebar sidebar = new RestSidebar();
            sidebar.buildBarracksEnabled = false;
            sidebar.buildMinigunnerEnabled = false;

            if (MikeAndConquerGame.instance.sidebarView.barracksSidebarIconView != null)
            {
                sidebar.buildBarracksEnabled = true;
            }

            if (MikeAndConquerGame.instance.sidebarView.minigunnerSidebarIconView != null)
            {
                sidebar.buildMinigunnerEnabled = true;
            }

            return Ok(sidebar);
        }



    }
}