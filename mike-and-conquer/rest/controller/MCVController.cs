using System.Web.Http;
using mike_and_conquer.rest.domain;
using Point = Microsoft.Xna.Framework.Point;


namespace mike_and_conquer.rest.controller
{

    public class MCVController : ApiController
    {
        public IHttpActionResult Get()
        {
            MCV mcv = GameWorld.instance.MCV;
            if (mcv == null)
            {
                return NotFound();
            }
            RestMCV restMCV = new RestMCV();

            restMCV.x = (int)mcv.positionInWorldCoordinates.X;
            restMCV.y = (int)mcv.positionInWorldCoordinates.Y;
            // Vector2 screenPosition =
            //     MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(minigunner
            //         .positionInWorldCoordinates);
            // restMinigunner.screenX = (int)screenPosition.X;
            // restMinigunner.screenY = (int)screenPosition.Y;
            // restMinigunner.health = minigunner.health;
            // restMinigunner.selected = minigunner.selected;
            // restMinigunner.destinationX = (int) minigunner.destination.X;
            // restMinigunner.destinationY = (int)minigunner.destination.Y;
            return Ok(restMCV);

        }

        // public IHttpActionResult Get(int id)
        // {
        //     Minigunner minigunner = GameWorld.instance.GetGDIMinigunnerByIdViaEvent(id);
        //     RestMinigunner restMinigunner = new RestMinigunner();
        //     restMinigunner.id = minigunner.id;
        //     restMinigunner.x = (int)minigunner.positionInWorldCoordinates.X;
        //     restMinigunner.y = (int)minigunner.positionInWorldCoordinates.Y;
        //     Vector2 screenPosition =
        //         MikeAndConquerGame.instance.ConvertWorldCoordinatesToScreenCoordinates(minigunner
        //             .positionInWorldCoordinates);
        //     restMinigunner.screenX = (int)screenPosition.X;
        //     restMinigunner.screenY = (int)screenPosition.Y;
        //     restMinigunner.health = minigunner.health;
        //     restMinigunner.selected = minigunner.selected;
        //     restMinigunner.destinationX = (int) minigunner.destination.X;
        //     restMinigunner.destinationY = (int)minigunner.destination.Y;
        //     return Ok(restMinigunner);
        // }


        public IHttpActionResult Post([FromBody]RestMCV inputMCV)
        {


            Point positionInWorldCoordinates = new Point(inputMCV.x, inputMCV.y);

            MCV mcv;

            // try
            // {
                mcv =
                    GameWorld.instance.CreateMCVViaEvent(positionInWorldCoordinates);
            // }
            // catch (BadMinigunnerLocationException e)
            // {
            //     return BadRequest("Cannot create on blocking terrain");
            // }

            RestMCV restMCV = new RestMCV();
            // restMCV.id = minigunner.id;
            restMCV.x = (int)mcv.positionInWorldCoordinates.X;
            restMCV.y = (int)mcv.positionInWorldCoordinates.Y;
            // restMCV.health = minigunner.health;

            // MikeAndConquerGame.instance.log.Information("leaving Post() for minigunner, id:" + restMinigunner.id);
            return Ok(restMCV);
        }

        // public void Put(int id, [FromBody]string value)
        // {
        // }
        //
        // public void Delete(int id)
        // {
        //     MikeAndConquerGame.instance.log.Information("Entering Delete() for minigunner, id:" + id);
        //     GameWorld.instance.SetGDIMinigunnerHealthViaEvent(id, 0);
        //     MikeAndConquerGame.instance.log.Information("Leaving Delete() for minigunner, id:" + id);
        // }
    }
}