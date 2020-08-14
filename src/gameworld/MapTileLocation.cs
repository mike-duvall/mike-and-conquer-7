
using Microsoft.Xna.Framework;
using mike_and_conquer.util;

namespace mike_and_conquer.gameworld
{
    public class MapTileLocation
    {
        private int xInWorldMapTileCoordinates;
        private int yInWorldMapTileCoordinates;



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
            Point mapTileCoordinates = ConvertWorldCoordinatesToMapTileCoordinates(worldCoordinatesInPoint);
            return new MapTileLocation(mapTileCoordinates.X, mapTileCoordinates.Y);
        }

        public static MapTileLocation CreateFromWorldCoordinates(int x, int y)
        {
            Point worldCoordinatesInPoint = new Point(x,y);
            Point mapTileCoordinates = ConvertWorldCoordinatesToMapTileCoordinates(worldCoordinatesInPoint);
            return new MapTileLocation(mapTileCoordinates.X, mapTileCoordinates.Y);
        }

//        public static MapTileLocation CreateCopy(MapTileLocation mapTileLocation)
//        {
//            return new MapTileLocation(mapTileLocation.xInWorldMapTileCoordinates, mapTileLocation.yInWorldMapTileCoordinates);
//        }


        public Point WorldMapTileCoordinatesAsPoint
        {
            get { return new Point(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates); }
        }



        public Point WorldCoordinatesAsPoint
        {
            get
            {
                return MapTileLocation.ConvertMapTileCoordinatesToWorldCoordinates(new Point(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates));
            }
        }


        public Vector2 WorldCoordinatesAsVector2
        {
            get
            {
                Point point = WorldCoordinatesAsPoint;
                return new Vector2(point.X, point.Y);
            }
        }


        public static Point ConvertMapTileCoordinatesToWorldCoordinates(Point pointInWorldMapSquareCoordinates)
        {

            int xInWorldCoordinates = (pointInWorldMapSquareCoordinates.X * GameWorld.MAP_TILE_WIDTH) +
                                      (GameWorld.MAP_TILE_WIDTH / 2);
            int yInWorldCoordinates = pointInWorldMapSquareCoordinates.Y * GameWorld.MAP_TILE_HEIGHT +
                                      (GameWorld.MAP_TILE_HEIGHT / 2);

            return new Point(xInWorldCoordinates, yInWorldCoordinates);
        }


        private static Point ConvertWorldCoordinatesToMapTileCoordinates(Point pointInWorldCoordinates)
        {

            int destinationRow = pointInWorldCoordinates.Y;
            int destinationColumn = pointInWorldCoordinates.X;

            int destinationX = destinationColumn / GameWorld.MAP_TILE_WIDTH;
            int destinationY = destinationRow / GameWorld.MAP_TILE_HEIGHT;

            return new Point(destinationX, destinationY);
        }

        public MapTileLocation Clone()
        {
            return new MapTileLocation(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates);
        }

        public MapTileLocation IncrementWorldMapTileX(int relativeX)
        {
            this.xInWorldMapTileCoordinates += relativeX;
            return this;
        }

        public MapTileLocation IncrementWorldMapTileY(int relativeY)
        {
            this.yInWorldMapTileCoordinates += relativeY;
            return this;
        }


        public MapTileLocation XInWorldCoordinates(int newX)
        {
            xInWorldMapTileCoordinates = newX / GameWorld.MAP_TILE_WIDTH;
            return this;
        }

        public MapTileLocation YInWorldCoordinates(int newY)
        {
            yInWorldMapTileCoordinates = newY / GameWorld.MAP_TILE_HEIGHT;
            return this;
        }

//        public int XInWorldCoordinates()
//        {
//            int xInWorldCoordinates = (xInWorldMapTileCoordinates * GameWorld.MAP_TILE_WIDTH) +
//                                      (GameWorld.MAP_TILE_WIDTH / 2);
//
//            return xInWorldCoordinates;
//        }


    }
}
