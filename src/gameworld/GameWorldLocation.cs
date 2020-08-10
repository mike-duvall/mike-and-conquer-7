
using Microsoft.Xna.Framework;
using mike_and_conquer.util;

namespace mike_and_conquer.gameworld
{
    public class GameWorldLocation
    {
        private float xInWorldCoordinates;
        private float yInWorldCoordinates;

        private GameWorldLocation(float x, float y)
        {
            this.xInWorldCoordinates = x;
            this.yInWorldCoordinates = y;
        }

//        public static GameWorldLocation CreateFromWorldMapTileCoordinates(int x, int y)
//        {
//            return new GameWorldLocation(x,y);
//        }
//
//        public static GameWorldLocation CreateFromWorldCoordinatesInVector2(Vector2 worldCoordinatesInVector2)
//        {
//            Point worldCoordinatesInPoint = PointUtil.ConvertVector2ToPoint(worldCoordinatesInVector2);
//            Point mapTileCoordinates = ConvertWorldCoordinatesToMapTileCoordinates(worldCoordinatesInPoint);
//            return new GameWorldLocation(mapTileCoordinates.X, mapTileCoordinates.Y);
//        }
//
//        public static GameWorldLocation CreateFromWorldCoordinates(int x, int y)
//        {
//            Point worldCoordinatesInPoint = new Point(x,y);
//            Point mapTileCoordinates = ConvertWorldCoordinatesToMapTileCoordinates(worldCoordinatesInPoint);
//            return new GameWorldLocation(mapTileCoordinates.X, mapTileCoordinates.Y);
//        }
//

        public static GameWorldLocation CreateFromWorldCoordinates(float x, float y)
        {
            return new GameWorldLocation(x,y);
        }
//
//        public Point WorldMapTileCoordinatesAsPoint
//        {
//            get { return new Point(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates); }
//        }
//
//
//
//        public Point WorldCoordinatesAsPoint
//        {
//            get
//            {
//                return MapTileLocation.ConvertMapTileCoordinatesToWorldCoordinates(new Point(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates));
//            }
//        }
//
//
        public Vector2 WorldCoordinatesAsVector2
        {
            get { return new Vector2(xInWorldCoordinates, yInWorldCoordinates); }
        }
//
//
//
//        public void UpdateLocationInWorldCoordinates(Point locationWordCoordinates)
//        {
//            Point locationInMapTileCoordinates = ConvertWorldCoordinatesToMapTileCoordinates(locationWordCoordinates);
//            xInWorldMapTileCoordinates = locationInMapTileCoordinates.X;
//            yInWorldMapTileCoordinates = locationInMapTileCoordinates.Y;
//        }
//
//
//        public static Point ConvertMapTileCoordinatesToWorldCoordinates(Point pointInWorldMapSquareCoordinates)
//        {
//
//            int xInWorldCoordinates = (pointInWorldMapSquareCoordinates.X * GameWorld.MAP_TILE_WIDTH) +
//                                      (GameWorld.MAP_TILE_WIDTH / 2);
//            int yInWorldCoordinates = pointInWorldMapSquareCoordinates.Y * GameWorld.MAP_TILE_HEIGHT +
//                                      (GameWorld.MAP_TILE_HEIGHT / 2);
//
//            return new Point(xInWorldCoordinates, yInWorldCoordinates);
//        }
//
//
//        private static Point ConvertWorldCoordinatesToMapTileCoordinates(Point pointInWorldCoordinates)
//        {
//
//            int destinationRow = pointInWorldCoordinates.Y;
//            int destinationColumn = pointInWorldCoordinates.X;
//
//            int destinationX = destinationColumn / GameWorld.MAP_TILE_WIDTH;
//            int destinationY = destinationRow / GameWorld.MAP_TILE_HEIGHT;
//
//            return new Point(destinationX, destinationY);
//        }



    }
}
