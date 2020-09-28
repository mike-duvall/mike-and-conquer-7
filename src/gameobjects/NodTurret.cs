using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;


namespace mike_and_conquer.gameobjects
{ 

    public class NodTurret
    {

        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }

        private int turretType;

        public int TurretType
        {
            get { return turretType; }
        }


        private float direction;

        public float Direction
        {
            get { return direction; }
        }


        protected NodTurret()
        {
        }


        public NodTurret(MapTileLocation mapTileLocation, int turretType, float direction)
        {
            this.mapTileLocation = mapTileLocation;
            this.turretType = turretType;
            this.direction = direction;
            this.previousDirection = direction;
        }

        private int view = 0;
        private int viewSwitchCounter = 0;

        private float previousDirection;

        public void Update(GameTime gameTime)
        {
//            viewSwitchCounter++;
//            if (viewSwitchCounter > 25)
//            {
//                viewSwitchCounter = 0;
//                view++;
//                if (view > 31)
//                {
//                    view = 0;
//                }
//
//            }
//
//            this.direction += 0.9f;
//            if (direction > 359.0f)
//            {
//                direction = 0f;
//            }
//
//            this.previousDirection = direction;
        }



        //        public bool ContainsPoint(Point aPoint)
        //        {
        //            int width = GameWorld.MAP_TILE_WIDTH;
        //            int height = GameWorld.MAP_TILE_HEIGHT;
        //
        //            int leftX = mapTileLocation.WorldCoordinatesAsPoint.X - (width / 2);
        //            int topY =  mapTileLocation.WorldCoordinatesAsPoint.Y - (height / 2);
        //
        //            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
        //            return boundRectangle.Contains(aPoint);
        //        }


    }


}
