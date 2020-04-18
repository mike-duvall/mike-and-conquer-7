using System;
using mike_and_conquer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using Minigunner = mike_and_conquer.gameobjects.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Point = Microsoft.Xna.Framework.Point;


namespace unit_tests
{

    public class UnitTestUtils
    {


        public static Point GetSlotLocationInWorldCoordinates(int slotNumber, Point locationInWorldCoordinates)
        {
            int slotDeltaX;
            int slotDeltaY;

            if (slotNumber == 0)
            {
                slotDeltaX = 4;
                slotDeltaY = -3;
            }
            else if (slotNumber == 1)
            {
                slotDeltaX = -8;
                slotDeltaY = -3;
            }
            else if (slotNumber == 2)
            {
                slotDeltaX = 4;
                slotDeltaY = 10;
            }
            else if (slotNumber == 3)
            {
                slotDeltaX = -8;
                slotDeltaY = 10;
            }
            else if (slotNumber == 4)
            {
                slotDeltaX = -2;
                slotDeltaY = 3;
            }
            else
            {
                throw new Exception("Invalid slot number:" + slotNumber);
            }

            locationInWorldCoordinates.X += slotDeltaX;
            locationInWorldCoordinates.Y += slotDeltaY;

            return locationInWorldCoordinates;


        }

        public static bool IsMinigunnerAtDestination(Minigunner minigunner, Point destination)
        {
            //            int leeway = 1;
            float leeway = 0.2f;
            bool isAtXDestination =
                (minigunner.positionInWorldCoordinates.X > destination.X - leeway) &&
                (minigunner.positionInWorldCoordinates.X < destination.X + leeway);

            bool isAtYDestination =
                (minigunner.positionInWorldCoordinates.Y > destination.Y - leeway) &&
                (minigunner.positionInWorldCoordinates.Y < destination.Y + leeway);


            return isAtXDestination && isAtYDestination;
        }






    }


}
