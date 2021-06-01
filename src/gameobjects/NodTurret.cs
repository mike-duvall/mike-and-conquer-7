using System;
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


        private Minigunner targetedMinigunner;
        private float goalDirection;

        private bool isCurrentlyTurningTowardsTarget = false;

        public static float TURN_ANGLE_SIZE = 360.0f / 32.0f;  // 11.25

//        private bool hasFired = false;

        private int fireDelay = 275;
        private int fireDelayCountdownTimer = -1;
        private int trackingDistance;


        // Turn rate in CnC is 12 on a scale of 0 to 256
        // Since MnC uses 360 degrees, need to convert that turn rate to one based on 360 degrees
        // private float baseTurnRate = 12.0f * 360.0f / 256.0f; // 16.875
        // Seems like above calculation would be right, but using empirical tests vs Cnc
        // Determined that turret turn speed needs to be 23
        private float baseTurnRate = 23f;


        public static float scaledTurnRate;

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
            this.trackingDistance = 24 * 7;
            this.id = NodTurret.globalId;
            NodTurret.globalId++;

        }


        private bool CanFire()
        {
            return fireDelayCountdownTimer < 0;
        }

        public void Update(GameTime gameTime)
        {

            fireDelayCountdownTimer--;
            EvaluateAndUpdateTargetedMinigunner();

            if (targetedMinigunner != null)
            {

                int distanceToTargedMinigunner = (int)Distance(MapTileLocation.WorldCoordinatesAsVector2.X,
                    MapTileLocation.WorldCoordinatesAsVector2.Y,
                    targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X,
                    targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);

                if (distanceToTargedMinigunner >= trackingDistance)
                {
                    targetedMinigunner = null;
                    return;
                }

                UpdateGoalDirection();
                EvaluateDirectionAndTurnIfNeeded(gameTime);

                if (IsPointingAtGoalDirection() && CanFire())
                {

                    Point myWorldCcCoordinatesAsPoint = this.MapTileLocation.WorldCoordinatesAsPoint;

                    GameWorldLocation projectileGameWorldLocation = 
                        GameWorldLocation.CreateFromWorldCoordinates(
                            myWorldCcCoordinatesAsPoint.X,
                            myWorldCcCoordinatesAsPoint.Y);


                    MikeAndConquerGame.instance.AddProjectile120mmAtGameWorldLocation(projectileGameWorldLocation, targetedMinigunner);
                    fireDelayCountdownTimer = fireDelay;
                }
            }
        }


        private void EvaluateDirectionAndTurnIfNeeded(GameTime gameTime)
        {

            float turnIncrement = CalculateTurnIncrement(gameTime);
            bool isPointingAtGoalDirection = IsPointingAtGoalDirection();
            if (!isPointingAtGoalDirection)
            {
                if (!isCurrentlyTurningTowardsTarget)
                {
                    isCurrentlyTurningTowardsTarget = true;
                }

                direction += turnIncrement;

            }
            else
            {
                isCurrentlyTurningTowardsTarget = false;
            }

            if (direction >= 360.0f)
            {
                direction = direction - 360.0f;
            }

            if (direction < 0.0f)
            {
                direction = 360.0f + direction;
            }


            //            MikeAndConquerGame.instance.log.Information("goalDirection:{0}, turnIncrement:{1}, isPointingAtGoalDirection:{2} ", goalDirection,
            //                turnIncrement, isPointingAtGoalDirection);

        }

        private void UpdateGoalDirection()
        {
            double targetDirectionFromTurret = GetAngle(
                targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.X,
                targetedMinigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);

            targetDirectionFromTurret += 90;
            if (targetDirectionFromTurret >= 360)
            {
                targetDirectionFromTurret = targetDirectionFromTurret - 360;
            }

            goalDirection = (float)targetDirectionFromTurret;


        }

        private void EvaluateAndUpdateTargetedMinigunner()
        {
            if (targetedMinigunner == null)
            {
                foreach (Minigunner minigunner in GameWorld.instance.GDIMinigunnerList)
                {
                    int distance = (int) Distance(MapTileLocation.WorldCoordinatesAsVector2.X,
                        MapTileLocation.WorldCoordinatesAsVector2.Y,
                        minigunner.GameWorldLocation.WorldCoordinatesAsVector2.X,
                        minigunner.GameWorldLocation.WorldCoordinatesAsVector2.Y);

                    if (distance < trackingDistance)
                    {
                        targetedMinigunner = minigunner;
                        return;
                    }
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

        private float CalculateTurnIncrement(GameTime gameTime)
        {

            scaledTurnRate = baseTurnRate / GameOptions.instance.CurrentGameSpeedDivisor();
            scaledTurnRate = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * scaledTurnRate);
            float clockWiseDistance = CalculateClockWiseDistance();

            if (clockWiseDistance >= 180.0f)
            {
                return -scaledTurnRate;
            }
            else
            {
                return scaledTurnRate;
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

            bool isPointingAsGoalDirection = Math.Abs(closestDistance) < TURN_ANGLE_SIZE / 2.0f;
            if (isPointingAsGoalDirection)
            {
                // snap to exact goalDirection
                direction = goalDirection;
            }
            return isPointingAsGoalDirection;

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

        private double Distance(double dX0, double dY0, double dX1, double dY1)
        {
            return Math.Sqrt((dX1 - dX0) * (dX1 - dX0) + (dY1 - dY0) * (dY1 - dY0));
        }

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
