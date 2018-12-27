
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class MinigunnerView
    {
        private GameSprite gameSprite;
        private UnitSelectionCursor unitSelectionCursor;
        private DestinationSquare destinationSquare;
        private Minigunner myMinigunner;

        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        protected MinigunnerView(Minigunner minigunner, string spriteListKey)
        {
            this.myMinigunner = minigunner;
            this.gameSprite = new GameSprite(spriteListKey);
            this.gameSprite.drawBoundingRectangle = false;
            this.unitSelectionCursor = new UnitSelectionCursor((int)this.myMinigunner.position.X, (int)this.myMinigunner.position.Y);
            this.destinationSquare = new DestinationSquare();

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

            gameSprite.AddAnimationSequence((int)AnimationSequences.WALKING_UP, walkingUpAnimationSequence);

            AnimationSequence standingStillAnimationSequence = new AnimationSequence(10);
            standingStillAnimationSequence.AddFrame(0);
            gameSprite.AddAnimationSequence((int)AnimationSequences.STANDING_STILL, standingStillAnimationSequence);
            gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);


            AnimationSequence shootinUpAnimationSequence = new AnimationSequence(10);
            shootinUpAnimationSequence.AddFrame(65);
            shootinUpAnimationSequence.AddFrame(66);
            shootinUpAnimationSequence.AddFrame(67);
            shootinUpAnimationSequence.AddFrame(68);
            shootinUpAnimationSequence.AddFrame(69);
            shootinUpAnimationSequence.AddFrame(70);
            shootinUpAnimationSequence.AddFrame(71);
            shootinUpAnimationSequence.AddFrame(72);
            gameSprite.AddAnimationSequence((int)AnimationSequences.SHOOTING_UP, shootinUpAnimationSequence);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(myMinigunner.health <= 0)
            {
                return;
            }

            unitSelectionCursor.position = new Vector2(myMinigunner.position.X, myMinigunner.position.Y);
            if (myMinigunner.state == Minigunner.State.IDLE)
            {
                gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);
            }
            else if (myMinigunner.state == Minigunner.State.MOVING)
            {
                gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
            }
            else if (myMinigunner.state == Minigunner.State.ATTACKING)
            {
                gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.SHOOTING_UP);
            }

            gameSprite.Draw(gameTime, spriteBatch, myMinigunner.position);

            if (myMinigunner.selected)
            {
                unitSelectionCursor.Draw(gameTime, spriteBatch);
            }

            if (this.myMinigunner.state == Minigunner.State.MOVING)
            {
                this.destinationSquare.position = this.myMinigunner.DestinationPosition;
                this.destinationSquare.Draw(gameTime, spriteBatch);
            }

        }

        public void SetAnimate(bool animateFlag)
        {
            gameSprite.SetAnimate(animateFlag);
        }



    }
}
