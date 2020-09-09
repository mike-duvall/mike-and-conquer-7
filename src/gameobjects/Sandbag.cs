using mike_and_conquer.gameworld;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer.gameobjects
{ 

    public class Sandbag
    {

        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }

        private int sandbagType;

        public int SandbagType
        {
            get { return sandbagType; }
        }



        protected Sandbag()
        {
        }


        public Sandbag(MapTileLocation mapTileLocation, int sandbagType)
        {
            this.mapTileLocation = mapTileLocation;
            this.sandbagType = sandbagType;
        }


        public bool ContainsPoint(Point aPoint)
        {
            int width = GameWorld.MAP_TILE_WIDTH;
            int height = GameWorld.MAP_TILE_HEIGHT;

            int leftX = mapTileLocation.WorldCoordinatesAsPoint.X - (width / 2);
            int topY =  mapTileLocation.WorldCoordinatesAsPoint.Y - (height / 2);

            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }


    }


}
