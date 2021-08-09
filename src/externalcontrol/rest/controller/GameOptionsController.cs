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
            if (gameSpeedAsString == "Slower") return GameOptions.GameSpeed.Slower;
            if (gameSpeedAsString == "Slow") return GameOptions.GameSpeed.Slow;
            if (gameSpeedAsString == "Moderate") return GameOptions.GameSpeed.Moderate;
            if (gameSpeedAsString == "Normal") return GameOptions.GameSpeed.Normal;
            if (gameSpeedAsString == "Fast") return GameOptions.GameSpeed.Fast;
            if (gameSpeedAsString == "Faster") return GameOptions.GameSpeed.Faster;
            if (gameSpeedAsString == "Fastest") return GameOptions.GameSpeed.Fastest;

            throw new Exception("Could not map game speed string of:" + gameSpeedAsString);
        }

        public IHttpActionResult Get()
        {
            GameOptions gameOptions = GameWorld.instance.GetGameOptionViaEvent();
            RestGameOptions restGameOptions = new RestGameOptions();
            restGameOptions.initialMapZoom = gameOptions.MapZoomLevel;
            // restGameOptions.GameSpeed = gameOptions.GameSpeedDelayDivisor.;
            restGameOptions.gameSpeed = ConvertGameSpeedValueToString(gameOptions.CurrentGameSpeed);

            restGameOptions.drawShroud = gameOptions.DrawShroud;

            return Ok(restGameOptions);
        }

        // private String ConvertGameSpeedValueToString(int gameSpeedDelayDivisor)
        // {
        //     if (gameSpeedDelayDivisor == (int) GameOptions.GameSpeed.Slowest) return "Slowest";
        //     if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Slower) return "Slower";
        //     if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Slow) return "Slow";
        //     if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Moderate) return "Moderate";
        //     if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Normal) return "Normal";
        //     if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Fast) return "Fast";
        //     if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Faster) return "Faster";
        //     if (gameSpeedDelayDivisor == (int)GameOptions.GameSpeed.Fastest) return "Fastest";
        //
        //
        //     throw new Exception("Could not map gameSpeedDivisor:" + gameSpeedDelayDivisor);
        // }

        private String ConvertGameSpeedValueToString(GameOptions.GameSpeed gameSpeed)
        {
            if (gameSpeed == GameOptions.GameSpeed.Slowest) return "Slowest";
            if (gameSpeed == GameOptions.GameSpeed.Slower) return "Slower";
            if (gameSpeed == GameOptions.GameSpeed.Slow) return "Slow";
            if (gameSpeed == GameOptions.GameSpeed.Moderate) return "Moderate";
            if (gameSpeed == GameOptions.GameSpeed.Normal) return "Normal";
            if (gameSpeed == GameOptions.GameSpeed.Fast) return "Fast";
            if (gameSpeed == GameOptions.GameSpeed.Faster) return "Faster";
            if (gameSpeed == GameOptions.GameSpeed.Fastest) return "Fastest";



            throw new Exception("Could not map gameSpeedDivisor:" + gameSpeed);
        }



    }
}
