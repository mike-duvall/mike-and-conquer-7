using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;


namespace mike_and_conquer.externalcontrol.rest.controller
{
    public class ResetGameController : ApiController
    {

        public void Post([FromBody]RestResetOptions resetOptions)
        {
            GameWorld.instance.ResetGameViaEvent(resetOptions.drawShroud, resetOptions.initialMapZoom, resetOptions.gameSpeedDelayDivisor);
        }


        public IHttpActionResult Get()
        {
            GameOptions2 gameOptions2 = GameWorld.instance.GetGameOptionViaEvent();
            RestResetOptions restResetOptions = new RestResetOptions();
            restResetOptions.initialMapZoom = gameOptions2.initialMapZoom;
            restResetOptions.gameSpeedDelayDivisor = gameOptions2.gameSpeedDelayDivisor;
            restResetOptions.drawShroud = gameOptions2.drawShroud;

            return Ok(restResetOptions);
        }


    }
}