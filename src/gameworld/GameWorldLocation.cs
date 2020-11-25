
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


        public static GameWorldLocation CreateFromWorldCoordinates(float x, float y)
        {
            return new GameWorldLocation(x, y);
        }

        public Vector2 WorldCoordinatesAsVector2
        {
            get { return new Vector2(xInWorldCoordinates, yInWorldCoordinates); }
        }


    }
}
