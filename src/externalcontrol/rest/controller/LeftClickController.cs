﻿using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using GameWorldView = mike_and_conquer.gameview.GameWorldView;

namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class LeftClickController : ApiController
    {


        public IHttpActionResult Post([FromBody]RestPoint point)
        {
            int screenWidth = GameWorldView.instance.ScreenWidth;
            int screenHeight = GameWorldView.instance.ScreenHeight;

            MouseInputHandler.DoLeftMouseClick( (uint)point.x, (uint)point.y, screenWidth, screenHeight);
            return Ok();
        }

    }
}