
using System;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;


namespace mike_and_conquer.gameobjects
{ 

    public class Projectile120mm
    {

        private GameWorldLocation gameWorldLocation;

        public GameWorldLocation GameWorldLocation
        {
            get { return gameWorldLocation; }
        }

        protected Projectile120mm()
        {
        }

        public int id { get; set; }
        private static int globalId = 1;

        private GameWorldLocation targetLocation;

        private int updateDelayTime = 5;
        private int updateDelayTimer;


        private static int baseCncSpeedInLeptons = (int)CncSpeed.MPH_VERY_FAST;  // MPH_VERY_FAST = 100
        private static readonly double baseMovementSpeedInWorldCoordinates = baseCncSpeedInLeptons * GameWorld.WorldUnitsPerLepton;
        private double scaledMovementSpeed;
        double movementDistanceEpsilon;

        private Minigunner target;



        // private static double baseSpeed = 9.375f; // MPH_VERY_FAST == 100, 
        // // private double movementVelocity = .68;
        //
        // private double movementVelocity = baseSpeed / GameOptions.GAME_SPEED_DELAY_DIVISOR;
        //
        //
        // double movementDistanceEpsilon;

        public Projectile120mm(GameWorldLocation startLocation, Minigunner target )
        {
            this.id = globalId;
            globalId++;
            this.gameWorldLocation = startLocation;
            this.target = target;
            this.targetLocation = target.GameWorldLocation;
//            this.updateDelayTime = 5;
            this.updateDelayTimer = updateDelayTime;
            // movementDistanceEpsilon = movementVelocity + (double).04f;

            scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / GameOptions.GAME_SPEED_DELAY_DIVISOR;
//            movementDistanceEpsilon = scaledMovementSpeed + (double).08f;
            movementDistanceEpsilon = scaledMovementSpeed + (double)3.2f;
            // movementDistanceEpsilon = 2.0;
                

        }



        public bool Update(GameTime gameTime)
        {

            scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / GameOptions.GAME_SPEED_DELAY_DIVISOR;

            // MikeAndConquerGame.instance.log.Information("movementVelocity:{0}, updateDelayTime{1} ", movementVelocity, updateDelayTime);
            bool removeMe = false;
            // updateDelayTimer--;
            // if (updateDelayTimer < 0)
            // {

                updateDelayTimer = updateDelayTime;

                float targetX = targetLocation.WorldCoordinatesAsVector2.X;
                float targetY = targetLocation.WorldCoordinatesAsVector2.Y;
                if (!IsAtDestination( targetX, targetY))
                {
                    MoveTowardsDestination(gameTime, targetX, targetY);
                }
                else
                {


                    SnapToTargetLocation();
                    
                    removeMe = true;
                // Cause damage to anything near the bullet impact point
                    target.ReduceHealth(10);
            }

            // }

            return removeMe;
        }


        private void SnapToTargetLocation()
        {
            gameWorldLocation =
                GameWorldLocation.CreateFromWorldCoordinates(
                    targetLocation.WorldCoordinatesAsVector2.X, targetLocation.WorldCoordinatesAsVector2.Y);

        }
        private bool IsAtDestination(float destinationX, float destinationY)
        {
            return IsAtDestinationX(destinationX) && IsAtDestinationY(destinationY);
        }

        private bool IsFarEnoughRight(float destinationX)
        {
            return (GameWorldLocation.WorldCoordinatesAsVector2.X > (destinationX - movementDistanceEpsilon));
        }

        private bool IsFarEnoughLeft(float destinationX)
        {
            return (GameWorldLocation.WorldCoordinatesAsVector2.X < (destinationX + movementDistanceEpsilon));
        }

        private bool IsFarEnoughDown(float destinationY)
        {
            return (GameWorldLocation.WorldCoordinatesAsVector2.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(float destinationY)
        {
            return (GameWorldLocation.WorldCoordinatesAsVector2.Y < (destinationY + movementDistanceEpsilon));
        }


        private bool IsAtDestinationX(float destinationX)
        {
            bool farEnoughRight = IsFarEnoughRight(destinationX);
            bool farEnoughLeft = IsFarEnoughLeft(destinationX);

            if(!(farEnoughRight && farEnoughLeft))
            {
                int x = 3;
            }

            return (
                IsFarEnoughRight(destinationX) &&
                IsFarEnoughLeft(destinationX)
            );

        }

        private bool IsAtDestinationY(float destinationY)
        {
            bool farEnoughDown = IsFarEnoughDown(destinationY);
            bool farEnoughUP = IsFarEnoughUp(destinationY);

            if (!(farEnoughDown && farEnoughUP))
            {
                int x = 3;
            }

            return (
                IsFarEnoughDown(destinationY) &&
                IsFarEnoughUp(destinationY)
            );

        }


        void MoveTowardsDestination(GameTime gameTime, float destinationX, float destinationY)
        {

            float newX = GameWorldLocation.WorldCoordinatesAsVector2.X;
            float newY = GameWorldLocation.WorldCoordinatesAsVector2.Y;

            // double delta = gameTime.ElapsedGameTime.TotalMilliseconds * movementVelocity;
            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * scaledMovementSpeed;

            Vector2 currentPosition = gameWorldLocation.WorldCoordinatesAsVector2;

            Vector2 directionVector = new Vector2(destinationX - currentPosition.X, destinationY - currentPosition.Y);
            directionVector.Normalize();

            // Position += Direction * Speed * gameTime.ElapsedGameTime.TotalSeconds;

            float deltaAsFloat = (float) delta;
            currentPosition = currentPosition + (directionVector * deltaAsFloat);

            gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(currentPosition.X, currentPosition.Y);


            // float remainingDistanceX = Math.Abs(destinationX - GameWorldLocation.WorldCoordinatesAsVector2.X);
            // float remainingDistanceY = Math.Abs(destinationY - GameWorldLocation.WorldCoordinatesAsVector2.Y);
            // double deltaX = delta;
            // double deltaY = delta;
            //
            // if (remainingDistanceX < deltaX)
            // {
            //     deltaX = remainingDistanceX;
            // }
            //
            // if (remainingDistanceY < deltaY)
            // {
            //     deltaY = remainingDistanceY;
            // }
            //
            // if (!IsFarEnoughRight(destinationX))
            // {
            //     newX += (float)deltaX;
            // }
            // else if (!IsFarEnoughLeft(destinationX))
            // {
            //     newX -= (float)deltaX;
            // }
            //
            // if (!IsFarEnoughDown(destinationY))
            // {
            //     newY += (float)deltaY;
            // }
            // else if (!IsFarEnoughUp(destinationY))
            // {
            //     newY -= (float)deltaY;
            // }
            //
            //
            // // TODO:  Leaving in this commented out code for debugging movement issues.
            // // Should remove it later if end up not needing it
            // //            float xChange = Math.Abs(positionInWorldCoordinates.X - newX);
            // //            float yChange = Math.Abs(positionInWorldCoordinates.Y - newY);
            // //            float changeThreshold = 0.10f;
            // //
            // //            if (xChange < changeThreshold && yChange < changeThreshold)
            // //            {
            // //                MikeAndConquerGame.instance.log.Information("delta:" + delta);
            // //                Boolean isFarEnoughRight = IsFarEnoughRight(destinationX);
            // //                Boolean isFarEnoughLeft = IsFarEnoughLeft(destinationX);
            // //                Boolean isFarEnoughDown = IsFarEnoughDown(destinationY);
            // //                Boolean isFarEnoughUp = IsFarEnoughUp(destinationY);
            // //
            // //                MikeAndConquerGame.instance.log.Information("isFarEnoughRight:" + isFarEnoughRight);
            // //                MikeAndConquerGame.instance.log.Information("isFarEnoughLeft:" + isFarEnoughLeft);
            // //                MikeAndConquerGame.instance.log.Information("isFarEnoughDown:" + isFarEnoughDown);
            // //                MikeAndConquerGame.instance.log.Information("isFarEnoughUp:" + isFarEnoughUp);
            // //                MikeAndConquerGame.instance.log.Information("old:positionInWorldCoordinates=" + positionInWorldCoordinates);
            // //                positionInWorldCoordinates = new Vector2(newX, newY);
            // //                MikeAndConquerGame.instance.log.Information("new:positionInWorldCoordinates=" + positionInWorldCoordinates);
            // //            }
            // //            else
            // //            {
            // //                positionInWorldCoordinates = new Vector2(newX, newY);
            // //            }
            //
            // gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(newX, newY);

        }



    }


}
