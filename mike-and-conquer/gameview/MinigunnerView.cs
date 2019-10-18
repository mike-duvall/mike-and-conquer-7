
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class MinigunnerView
    {
        private UnitSprite unitSprite;
        private UnitSelectionCursor unitSelectionCursor;
        private DestinationSquare destinationSquare;
        private Minigunner myMinigunner;
        private bool drawDestinationSquare;

        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        protected MinigunnerView(Minigunner minigunner, string spriteListKey)
        {
            this.myMinigunner = minigunner;
            this.unitSprite = new UnitSprite(spriteListKey);
            this.unitSprite.drawBoundingRectangle = false;
            this.unitSprite.drawShadow = true;
            this.unitSelectionCursor = new UnitSelectionCursor((int)this.myMinigunner.positionInWorldCoordinates.X, (int)this.myMinigunner.positionInWorldCoordinates.Y);
            this.destinationSquare = new DestinationSquare();
            this.drawDestinationSquare = false;
            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence walkingUpAnimationSequence = new AnimationSequence(20);
            walkingUpAnimationSequence.AddFrame(16);
            walkingUpAnimationSequence.AddFrame(17);
            walkingUpAnimationSequence.AddFrame(18);
            walkingUpAnimationSequence.AddFrame(19);
            walkingUpAnimationSequence.AddFrame(20);
            walkingUpAnimationSequence.AddFrame(21);

            unitSprite.AddAnimationSequence((int)AnimationSequences.WALKING_UP, walkingUpAnimationSequence);

            AnimationSequence standingStillAnimationSequence = new AnimationSequence(10);
            standingStillAnimationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence((int)AnimationSequences.STANDING_STILL, standingStillAnimationSequence);
            unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);


            AnimationSequence shootinUpAnimationSequence = new AnimationSequence(10);
            shootinUpAnimationSequence.AddFrame(65);
            shootinUpAnimationSequence.AddFrame(66);
            shootinUpAnimationSequence.AddFrame(67);
            shootinUpAnimationSequence.AddFrame(68);
            shootinUpAnimationSequence.AddFrame(69);
            shootinUpAnimationSequence.AddFrame(70);
            shootinUpAnimationSequence.AddFrame(71);
            shootinUpAnimationSequence.AddFrame(72);
            unitSprite.AddAnimationSequence((int)AnimationSequences.SHOOTING_UP, shootinUpAnimationSequence);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(myMinigunner.health <= 0)
            {
                return;
            }

            unitSelectionCursor.position = new Vector2(myMinigunner.positionInWorldCoordinates.X, myMinigunner.positionInWorldCoordinates.Y);
            if (myMinigunner.state == Minigunner.State.IDLE)
            {
                unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);
            }
            else if (myMinigunner.state == Minigunner.State.MOVING)
            {
                unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
            }
            else if (myMinigunner.state == Minigunner.State.ATTACKING)
            {
                unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.SHOOTING_UP);
            }

            unitSprite.Draw(gameTime, spriteBatch, myMinigunner.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);

            if (myMinigunner.selected)
            {
                unitSelectionCursor.Draw(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }

            if (this.drawDestinationSquare && this.myMinigunner.state == Minigunner.State.MOVING)
            {
                this.destinationSquare.position = this.myMinigunner.DestinationPosition;
                this.destinationSquare.Draw(gameTime, spriteBatch);
            }

        }

        public void SetAnimate(bool animateFlag)
        {
            unitSprite.SetAnimate(animateFlag);
        }



    }
}
