using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using mike_and_conquer.util;

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

        public static MapTileLocation CreateFromWorldCoordinatesInVector2(Vector2 worldCoordinatesInVector2)
        {
            Point worldCoordinatesInPoint = PointUtil.ConvertVector2ToPoint(worldCoordinatesInVector2);
            Point mapTileCoordinates = MapTileLocation.ConvertWorldCoordinatesToMapTileCoordinates(worldCoordinatesInPoint);
            return new MapTileLocation(mapTileCoordinates.X, mapTileCoordinates.Y);
        }

        public static MapTileLocation CreateFromWorldCoordinates(int x, int y)
        {
            Point worldCoordinatesInPoint = new Point(x,y);
            Point mapTileCoordinates = MapTileLocation.ConvertWorldCoordinatesToMapTileCoordinates(worldCoordinatesInPoint);
            return new MapTileLocation(mapTileCoordinates.X, mapTileCoordinates.Y);
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
            Point locationInMapTileCoordinates = ConvertWorldCoordinatesToMapTileCoordinates(locationWordCoordinates);
            xInWorldMapTileCoordinates = locationInMapTileCoordinates.X;
            yInWorldMapTileCoordinates = locationInMapTileCoordinates.Y;
        }


        public static Point ConvertMapTileCoordinatesToWorldCoordinates(Point pointInWorldMapSquareCoordinates)
        {

            int xInWorldCoordinates = (pointInWorldMapSquareCoordinates.X * GameWorld.MAP_TILE_WIDTH) +
                                      (GameWorld.MAP_TILE_WIDTH / 2);
            int yInWorldCoordinates = pointInWorldMapSquareCoordinates.Y * GameWorld.MAP_TILE_HEIGHT +
                                      (GameWorld.MAP_TILE_HEIGHT / 2);

            return new Point(xInWorldCoordinates, yInWorldCoordinates);
        }


        public static Point ConvertWorldCoordinatesToMapTileCoordinates(Point pointInWorldCoordinates)
        {

            int destinationRow = pointInWorldCoordinates.Y;
            int destinationColumn = pointInWorldCoordinates.X;

            int destinationX = destinationColumn / GameWorld.MAP_TILE_WIDTH;
            int destinationY = destinationRow / GameWorld.MAP_TILE_HEIGHT;

            return new Point(destinationX, destinationY);
        }


    }
}
