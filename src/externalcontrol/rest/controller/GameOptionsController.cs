using System;
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
            GameOptions.GameSpeed gameSpeed = ConvertGameSpeedStringToEnum(gameOptions.gameSpeed);
            
            // GameWorld.instance.ResetGameViaEvent(gameOptions.DrawShroud, gameOptions.InitialMapZoom, gameOptions.gameSpeedDelayDivisor);
            GameWorld.instance.ResetGameViaEvent(gameOptions.drawShroud, gameOptions.initialMapZoom, gameSpeed);
        }


        private GameOptions.GameSpeed ConvertGameSpeedStringToEnum(String gameSpeedAsString)
        {
            if (gameSpeedAsString == "Slowest") return GameOptions.GameSpeed.Slowest;
            if (gameSpeedAsString == "Normal") return GameOptions.GameSpeed.Normal;
            if (gameSpeedAsString == "Fastest") return GameOptions.GameSpeed.Fastest;

            throw new Exception("Could not map game speed string of:" + gameSpeedAsString);
        }

        public IHttpActionResult Get()
        {
            GameOptions gameOptions = GameWorld.instance.GetGameOptionViaEvent();
            RestGameOptions restGameOptions = new RestGameOptions();
            restGameOptions.initialMapZoom = gameOptions.MapZoomLevel;
            // restGameOptions.GameSpeed = gameOptions.GameSpeedDelayDivisor.;
            restGameOptions.gameSpeed = ConvertGameSpeedValueToString(gameOptions.GameSpeedDelayDivisor);

            restGameOptions.drawShroud = gameOptions.DrawShroud;

            return Ok(restGameOptions);
        }

        private String ConvertGameSpeedValueToString(int gameSpeedDelayDivisor)
        {
            if (gameSpeedDelayDivisor == (int) GameOptions.GameSpeed.Slowest) return "Slowest";
            if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Normal) return "Normal";
            if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Fastest) return "Fastest";

            throw new Exception("Could not map gameSpeedDivisor:" + gameSpeedDelayDivisor);
        }


    }
}