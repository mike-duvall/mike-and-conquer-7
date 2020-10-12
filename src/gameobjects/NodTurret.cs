﻿using System;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;


namespace mike_and_conquer.gameobjects
{ 

    public class NodTurret
    {


        public int id { get;  }

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


        private static int globalId = 1;


        private float goalDirection;

        private Minigunner targetedMinigunner;

        private bool isCurrentlyTurningTowardsTarget = false;
        private int turnDelay = 15;
        private int turnDelayCountdownTimer = -1;
        private float turnIncrement;

        public static float TURN_ANGLE_SIZE = 360.0f / 32.0f;  // 11.25



        protected NodTurret()
        {
        }


        public NodTurret(MapTileLocation mapTileLocation, int turretType, float direction)
        {
            this.mapTileLocation = mapTileLocation;
            this.turretType = turretType;
            this.direction = direction;
            this.goalDirection = direction;
            this.targetedMinigunner = null;
            this.id = NodTurret.globalId;
            NodTurret.globalId++;

        }



        public void Update(GameTime gameTime)
        {
            if (targetedMinigunner == null)
            {
                foreach (Minigunner minigunner in GameWorld.instance.GDIMinigunnerList)
                {
                    int distance = (int)Distance(MapTileLocation.WorldCoordinatesAsVector2.X,
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

                int distance = (int)Distance(MapTileLocation.WorldCoordinatesAsVector2.X,
                    MapTileLocation.WorldCoordinatesAsVector2.Y,
                    targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X,
                    targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);

                int trackingDistance = 24 * 5;
                if (distance >= trackingDistance)
                {
                    targetedMinigunner = null;
                    return;
                }

                double angle = GetAngle(
                    targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X,
                    targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);

                angle += 90;
                if (angle > 360)
                {
                    angle = angle - 360;
                }

                goalDirection = (float)angle;
                turnIncrement = CalculateTurnIncrement();
                bool isPointingAtGoalDirection = IsPointingAtGoalDirection();
                MikeAndConquerGame.instance.log.Information("goalDirection:{0}, turnIncrement:{1}, isPointingAtGoalDirection:{2} ", goalDirection,
                    turnIncrement, isPointingAtGoalDirection);

                if (!isPointingAtGoalDirection)
                {
                    if (!isCurrentlyTurningTowardsTarget)
                    {
                        isCurrentlyTurningTowardsTarget = true;
                        turnDelayCountdownTimer = turnDelay;
                    }

                    turnDelayCountdownTimer--;
                    if (turnDelayCountdownTimer <= 0)
                    {
                        turnDelayCountdownTimer = turnDelay;
                        direction += turnIncrement;
                    }
                    if (direction >= 360.0f)
                    {
                        direction = direction - 360.0f;
                    }

                    if (direction < 0.0f)
                    {
                        direction = 360.0f + direction;
                    }

                }
                else
                {
                    isCurrentlyTurningTowardsTarget = false;
                }


            }

        }


        private float CalculateClockWiseDistance()
        {
            float clockWiseDistance = 0f;
            if (goalDirection > direction)
            {
                clockWiseDistance = goalDirection - direction;
            }
            else
            {
                clockWiseDistance = (360.0f - direction) + goalDirection;
            }

            return clockWiseDistance;

        }

        private float CalculateTurnIncrement()
        {
            float clockWiseDistance = CalculateClockWiseDistance();

            if (clockWiseDistance >= 180.0f)
            {
                return -TURN_ANGLE_SIZE;
            }
            else
            {
                return TURN_ANGLE_SIZE;
            }

        }

        private bool IsPointingAtGoalDirection()
        {
            float clockwiseDistance = CalculateClockWiseDistance();
            float counterClockwiseDistance = 360.0f - clockwiseDistance;

            float closestDistance = 0.0f;
            if (clockwiseDistance <= counterClockwiseDistance)
            {
                closestDistance = clockwiseDistance;
            }
            else
            {
                closestDistance = counterClockwiseDistance;
            }

            return Math.Abs(closestDistance) < TURN_ANGLE_SIZE / 2.0f;
        }

//        public static bool NearlyEqual(float f1, float f2, float epsilon)
//        {
//            // Equal if they are within 0.00001 of each other
//            return Math.Abs(f1 - f2) < epsilon;
//        }

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