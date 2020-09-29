using System;
using Microsoft.Xna.Framework;
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


        private float direction;

        public float Direction
        {
            get { return direction; }
        }

        private Minigunner targetedMinigunner;

        protected NodTurret()
        {
        }


        public NodTurret(MapTileLocation mapTileLocation, int turretType, float direction)
        {
            this.mapTileLocation = mapTileLocation;
            this.turretType = turretType;
            this.direction = direction;
            this.previousDirection = direction;
            this.targetedMinigunner = null;
        }

        private int view = 0;
        private int viewSwitchCounter = 0;

        private float previousDirection;


        public void Update(GameTime gameTime)
        {
//            viewSwitchCounter++;
//            if (viewSwitchCounter > 25)
//            {
//                viewSwitchCounter = 0;
//                view++;
//                if (view > 31)
//                {
//                    view = 0;
//                }
//
//            }
//
//            this.direction += 0.9f;
//            if (direction > 359.0f)
//            {
//                direction = 0f;
//            }
//
//            this.previousDirection = direction;
            if (targetedMinigunner == null)
            {
                foreach(Minigunner minigunner in GameWorld.instance.GDIMinigunnerList)
                {
                    int distance = (int) Distance(MapTileLocation.WorldCoordinatesAsVector2.X,
                        MapTileLocation.WorldCoordinatesAsVector2.Y,
                        minigunner.GameWorldLocation.WorldCoordinatesAsVector2.X,
                        minigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);

                    int trackingDistance = 24 * 5;
                    if (distance < trackingDistance)
                    {
                        targetedMinigunner = minigunner;
                        break;
                    }
                }
            }

            if (targetedMinigunner != null)
            {
                double angle = GetAngle(
                    targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X,
                    targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);

                angle += 90;
                if (angle > 360)
                {
                    angle = angle - 360;
                }
                direction = (int) angle;

            }
        }


        public static double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }

        public double GetAngle(float x, float y)
        {
            double dx = x - MapTileLocation.WorldCoordinatesAsVector2.X;
            // Minus to correct for coord re-mapping
            double dy = -(y - MapTileLocation.WorldCoordinatesAsVector2.Y);

            double inRads = Math.Atan2(dy, dx);

            // We need to map to coord system when 0 degree is at 3 O'clock, 270 at 12 O'clock
            if (inRads < 0)
                inRads = Math.Abs(inRads);
            else
                inRads = 2 * Math.PI - inRads;


            return ConvertRadiansToDegrees(inRads);


        }

        //        public double getAngle(Point screenPoint)
        //        {
        //            double dx = screenPoint.getX() - mCentreX;
        //            // Minus to correct for coord re-mapping
        //            double dy = -(screenPoint.getY() - mCentreY);
        //
        //            double inRads = Math.atan2(dy, dx);
        //
        //            // We need to map to coord system when 0 degree is at 3 O'clock, 270 at 12 O'clock
        //            if (inRads < 0)
        //                inRads = Math.abs(inRads);
        //            else
        //                inRads = 2 * Math.PI - inRads;
        //
        //            return Math.toDegrees(inRads);
        //        }
        //
        private double Distance(double dX0, double dY0, double dX1, double dY1)
        {
            return Math.Sqrt((dX1 - dX0) * (dX1 - dX0) + (dY1 - dY0) * (dY1 - dY0));
        }


//        private int CalculateDistanceToTarget()
//        {
//            return (int)Distance(
//                MapTileLocation.WorldCoordinatesAsVector2.X,
//                MapTileLocation.WorldCoordinatesAsVector2.Y,
//                targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X,
//                targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);
//        }



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
