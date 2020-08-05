using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace mike_and_conquer.gameworld
{
    public class MapTileLocation
    {
        private int xInWorldMapTileCoordinates;
        private int yInWorldMapTileCoordinates;



        public int XinWorldMapTileCoordinates
        {
            get { return xInWorldMapTileCoordinates; }
            set { xInWorldMapTileCoordinates = value; }
        }


        public int YinWorldMapTileCoordinates
        {
            get { return yInWorldMapTileCoordinates; }
            set { yInWorldMapTileCoordinates = value; }
        }




        private MapTileLocation(int x, int y)
        {
            this.xInWorldMapTileCoordinates = x;
            this.yInWorldMapTileCoordinates = y;
        }

        public static MapTileLocation CreateFromWorldMapTileCoordinates(int x, int y)
        {
            return new MapTileLocation(x,y);
        }

//        public Point ToPoint()
//        {
//            return new Point(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates);
//        }
        public float XInWorldCoordinates
        {
            get { return (xInWorldMapTileCoordinates * GameWorld.MAP_TILE_WIDTH) + (GameWorld.MAP_TILE_WIDTH / 2); }
        }

        public float YInWorldCoordinates
        {
            get { return (yInWorldMapTileCoordinates * GameWorld.MAP_TILE_HEIGHT) + (GameWorld.MAP_TILE_HEIGHT / 2); }
        }

        public Vector2 WorldCoordinatesAsVector2
        {
            get
            {
                return new Vector2(XInWorldCoordinates, YInWorldCoordinates);
            }
        }


        public Point WorldCoordinatesAsPoint
        {
            get
            {
                return new Point((int) XInWorldCoordinates, (int) YInWorldCoordinates);
            }
        }

        public void UpdateLocationInWorldCoordinates(Point locationWordCoordinates)
        {
            Point locationInMapTileCoordinates = GameWorld.instance.ConvertWorldCoordinatesToMapTileCoordinates(locationWordCoordinates);
            xInWorldMapTileCoordinates = locationInMapTileCoordinates.X;
            yInWorldMapTileCoordinates = locationInMapTileCoordinates.Y;
        }
    }
}
