
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

        private static int baseCncSpeedInLeptons = (int)CncSpeed.MPH_VERY_FAST;  // MPH_VERY_FAST = 100
        private static readonly double baseMovementSpeedInWorldCoordinates = baseCncSpeedInLeptons * GameWorld.WorldUnitsPerLepton;
        private double scaledMovementSpeed;
        double movementDistanceEpsilon;

        private Minigunner target;


        public Projectile120mm(GameWorldLocation startLocation, Minigunner target )
        {
            this.id = globalId;
            globalId++;
            this.gameWorldLocation = startLocation;
            this.target = target;
            this.targetLocation = target.GameWorldLocation;

            scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / GameOptions.instance.CurrentGameSpeedDivisor();
//            movementDistanceEpsilon = scaledMovementSpeed + (double).08f;
            movementDistanceEpsilon = scaledMovementSpeed + (double)3.2f;
            // movementDistanceEpsilon = 2.0;
                

        }



        public bool Update(GameTime gameTime)
        {

            scaledMovementSpeed = baseMovementSpeedInWorldCoordinates / GameOptions.instance.CurrentGameSpeedDivisor();

            // MikeAndConquerGame.instance.log.Information("movementVelocity:{0}, updateDelayTime{1} ", movementVelocity, updateDelayTime);
            bool removeMe = false;

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
                target.ReduceHealth(10);
            }

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
            return (
                IsFarEnoughRight(destinationX) &&
                IsFarEnoughLeft(destinationX)
            );

        }

        private bool IsAtDestinationY(float destinationY)
        {
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



        }



    }


}
