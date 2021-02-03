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

    public class GdiMinigunnersController : ApiController
    {
        public IEnumerable<RestMinigunner> Get()
        {

            RestMinigunner[] restMinigunners = new RestMinigunner[GameWorld.instance.GDIMinigunnerList.Count];

            int i = 0;
            // Bogus, this should get the minigunner list via Event instead of directly
            foreach (Minigunner minigunner in GameWorld.instance.GDIMinigunnerList)
            {
                RestMinigunner restMinigunner = new RestMinigunner();
                restMinigunner.id = minigunner.id;
                restMinigunner.x = (int)minigunner.GameWorldLocation.WorldCoordinatesAsVector2.X;
                restMinigunner.y = (int)minigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y;
                Vector2 screenPosition =
                    GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(minigunner
                        .GameWorldLocation.WorldCoordinatesAsVector2);
                restMinigunner.screenX = (int)screenPosition.X;
                restMinigunner.screenY = (int)screenPosition.Y;
                restMinigunner.health = minigunner.health;
                restMinigunner.selected = minigunner.selected;
                restMinigunner.destinationX = (int)minigunner.destination.X;
                restMinigunner.destinationY = (int)minigunner.destination.Y;
                restMinigunners[i] = restMinigunner;
            }

            // return Ok(restMinigunners);
            return restMinigunners;
        }

        public IHttpActionResult Get(int id)
        {
            // Bogus:  Even though we are getting this minigunner by event, we get a reference
            // to the actual minigunner, not a copy, so in theory this minigunner could be getting
            // updated in the game thread while we are using it
            Minigunner minigunner = GameWorld.instance.GetGDIMinigunnerByIdViaEvent(id);
            RestMinigunner restMinigunner = new RestMinigunner();
            restMinigunner.id = minigunner.id;
            restMinigunner.x = (int)minigunner.GameWorldLocation.WorldCoordinatesAsVector2.X;
            restMinigunner.y = (int)minigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y;
            Vector2 screenPosition =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(minigunner
                    .GameWorldLocation.WorldCoordinatesAsVector2);
            restMinigunner.screenX = (int)screenPosition.X;
            restMinigunner.screenY = (int)screenPosition.Y;
            restMinigunner.health = minigunner.health;
            restMinigunner.selected = minigunner.selected;
            restMinigunner.destinationX = (int) minigunner.destination.X;
            restMinigunner.destinationY = (int)minigunner.destination.Y;
            return Ok(restMinigunner);
        }


        public IHttpActionResult Post([FromBody]RestMinigunner inputMinigunner)
        {


            Point minigunnerPositionInWorldCoordinates = new Point(inputMinigunner.x, inputMinigunner.y);

            Minigunner minigunner;

            try
            {
                minigunner =
                    GameWorld.instance.CreateGDIMinigunnerViaEvent(minigunnerPositionInWorldCoordinates);
            }
            catch (BadMinigunnerLocationException)
            {
                return BadRequest("Cannot create on blocking terrain");
            }

            RestMinigunner restMinigunner = new RestMinigunner();
            restMinigunner.id = minigunner.id;
            restMinigunner.x = (int)minigunner.GameWorldLocation.WorldCoordinatesAsVector2.X;
            restMinigunner.y = (int)minigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y;
            restMinigunner.health = minigunner.health;

            // MikeAndConquerGame.instance.log.Information("leaving Post() for minigunner, id:" + restMinigunner.id);
            return Ok(restMinigunner);
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
            GameWorld.instance.SetGDIMinigunnerHealthViaEvent(id, 0);
        }
    }
}