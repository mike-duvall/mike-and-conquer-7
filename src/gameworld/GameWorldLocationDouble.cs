
using Microsoft.Xna.Framework;
using mike_and_conquer.util;

namespace mike_and_conquer.gameworld
{
    public class GameWorldLocationDouble
    {
        private double xInWorldCoordinates;
        private double yInWorldCoordinates;

        private GameWorldLocationDouble(double x, double y)
        {
            this.xInWorldCoordinates = x;
            this.yInWorldCoordinates = y;
        }


        public static GameWorldLocationDouble CreateFromWorldCoordinates(double x, double y)
        {
            return new GameWorldLocationDouble(x, y);
        }


        public double X
        {
            get { return xInWorldCoordinates; }
        }

        public double Y
        {
            get { return yInWorldCoordinates; }
        }


        // public Vector2 WorldCoordinatesAsVector2
        // {
        //     get { return new Vector2(xInWorldCoordinates, yInWorldCoordinates); }
        // }


    }
}
