using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
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
            GameOptions gameOptions = GameWorld.instance.GetGameOptionViaEvent();
            RestResetOptions restResetOptions = new RestResetOptions();
            restResetOptions.initialMapZoom = gameOptions.MapZoomLevel;
            restResetOptions.gameSpeedDelayDivisor = gameOptions.GameSpeedDelayDivisor;
            restResetOptions.drawShroud = gameOptions.DrawShroud;

            return Ok(restResetOptions);
        }


    }
}