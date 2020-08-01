using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace mike_and_conquer.gameworld
{
    public class GameLocation
    {
        private int xInWorldCoordinates;
        private int yInWorldCoordinates;



        public int X
        {
            get { return xInWorldCoordinates; }
            set { xInWorldCoordinates = value; }
        }


        public int Y
        {
            get { return yInWorldCoordinates; }
            set { yInWorldCoordinates = value; }
        }


        private GameLocation(int x, int y)
        {
            this.xInWorldCoordinates = x;
            this.yInWorldCoordinates = y;
        }

        public static GameLocation CreateGameLocationInWorldCoordinates(int x, int y)
        {
            return new GameLocation(x,y);
        }

        public Point ToPoint()
        {
            return new Point(xInWorldCoordinates, yInWorldCoordinates);
        }
    }
}
