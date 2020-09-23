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


        private int direction;

        public int Direction
        {
            get { return Direction; }
        }


        protected NodTurret()
        {
        }


        public NodTurret(MapTileLocation mapTileLocation, int turretType, int direction)
        {
            this.mapTileLocation = mapTileLocation;
            this.turretType = turretType;
            this.direction = direction;
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
