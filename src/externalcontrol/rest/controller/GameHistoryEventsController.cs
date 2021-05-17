using System.Collections.Generic;
using System.Web.Http;
using mike_and_conquer.externalcontrol.rest.domain;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using BadMinigunnerLocationException = mike_and_conquer.gameworld.GameWorld.BadMinigunnerLocationException;

using GameWorldView = mike_and_conquer.gameview.GameWorldView;


namespace mike_and_conquer.externalcontrol.rest.controller
{

    public class GameHistoryEventsController : ApiController
    {
        public IEnumerable<RestGameHistoryEvent> Get()
        {


            RestGameHistoryEvent[] restGameHistoryEvents =
                new RestGameHistoryEvent[GameWorld.instance.GameHistoryEventList.Count];

            // int i = 0;
            // // Bogus, this should get the minigunner list via Event instead of directly
            // foreach (Minigunner minigunner in GameWorld.instance.GDIMinigunnerList)
            // {
            //     RestMinigunner restMinigunner = new RestMinigunner();
            //     restMinigunner.id = minigunner.id;
            //     restMinigunner.x = (int)minigunner.GameWorldLocation.WorldCoordinatesAsVector2.X;
            //     restMinigunner.y = (int)minigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y;
            //     Vector2 screenPosition =
            //         GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(minigunner
            //             .GameWorldLocation.WorldCoordinatesAsVector2);
            //     restMinigunner.screenX = (int)screenPosition.X;
            //     restMinigunner.screenY = (int)screenPosition.Y;
            //     restMinigunner.health = minigunner.Health;
            //     restMinigunner.selected = minigunner.selected;
            //     restMinigunner.destinationX = (int)minigunner.destination.X;
            //     restMinigunner.destinationY = (int)minigunner.destination.Y;
            //     restMinigunners[i] = restMinigunner;
            //     i++;
            // }

            int i = 0;
            // Bogus, this should get the minigunner list via Event instead of directly
            foreach (GameHistoryEvent gameHistoryEvent in GameWorld.instance.GameHistoryEventList)
            {

                RestGameHistoryEvent restGameHistoryEvent = new RestGameHistoryEvent();
                restGameHistoryEvent.eventType = gameHistoryEvent.EventType;
                restGameHistoryEvent.unitId = gameHistoryEvent.UnitId;
                restGameHistoryEvent.wallClockTime = gameHistoryEvent.WallClockTime;
                restGameHistoryEvents[i] = restGameHistoryEvent;
                i++;
            }


            // RestGameHistoryEvent restGameHistoryEvent = new RestGameHistoryEvent();
            // restGameHistoryEvent.eventType = "FirePrimaryWeapon";
            // restGameHistoryEvents[0] = restGameHistoryEvent;
            // return Ok(restMinigunners);
            return restGameHistoryEvents;
        }


    }
}