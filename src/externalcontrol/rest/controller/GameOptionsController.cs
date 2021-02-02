using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;


namespace mike_and_conquer.externalcontrol.rest.controller
{
    public class GameOptionsController : ApiController
    {

        public void Post([FromBody]RestGameOptions gameOptions)
        {
            GameWorld.instance.ResetGameViaEvent(gameOptions.drawShroud, gameOptions.initialMapZoom, gameOptions.gameSpeedDelayDivisor);
        }


        public IHttpActionResult Get()
        {
            GameOptions gameOptions = GameWorld.instance.GetGameOptionViaEvent();
            RestGameOptions restGameOptions = new RestGameOptions();
            restGameOptions.initialMapZoom = gameOptions.MapZoomLevel;
            restGameOptions.gameSpeedDelayDivisor = gameOptions.GameSpeedDelayDivisor;
            restGameOptions.drawShroud = gameOptions.DrawShroud;

            return Ok(restGameOptions);
        }


    }
}