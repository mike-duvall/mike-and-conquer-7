using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.externalcontrol.rest.controller
{
    public class ResetGameController : ApiController
    {

        public void Post([FromBody]RestResetOptions resetOptions)
        {
            GameWorld.instance.ResetGameViaEvent(resetOptions.drawShroud, resetOptions.initialMapZoom, resetOptions.gameSpeedDelayDivisor);
        }


    }
}