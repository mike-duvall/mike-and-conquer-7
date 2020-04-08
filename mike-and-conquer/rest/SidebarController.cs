﻿
using System.Web.Http;
using mike_and_conquer.rest.domain;

namespace mike_and_conquer.rest
{

    public class SidebarController : ApiController
    {

        public IHttpActionResult Get()
        {
            RestSidebar sidebar = new RestSidebar();
            sidebar.buildBarracksEnabled = false;
            sidebar.buildMinigunnerEnabled = false;

            if (MikeAndConquerGame.instance.barracksToolbarIconView != null)
            {
                sidebar.buildBarracksEnabled = true;
            }

            if (MikeAndConquerGame.instance.minigunnerIconView != null)
            {
                sidebar.buildMinigunnerEnabled = true;
            }

            return Ok(sidebar);
        }



    }
}