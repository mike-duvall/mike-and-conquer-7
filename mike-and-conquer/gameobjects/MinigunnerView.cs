
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Math = System.Math;

namespace mike_and_conquer.gameobjects
{
    public class MinigunnerView
    {
        private GameSprite gameSprite;
        private UnitSelectionCursor unitSelectionCursor;
        private Minigunner myMinigunner;

        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        public MinigunnerView(Minigunner minigunner, string spriteListKey)
        {
            this.myMinigunner = minigunner;
            this.gameSprite = new GameSprite(spriteListKey);
            this.unitSelectionCursor = new UnitSelectionCursor((int)this.myMinigunner.position.X, (int)this.myMinigunner.position.Y);
            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence walkingUpAnimationSequence = new AnimationSequence(10);
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


        public void Update()
        {
            unitSelectionCursor.position = new Vector2(myMinigunner.position.X, myMinigunner.position.Y);
            if(myMinigunner.State == "IDLE" )
            {
                gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);
            }
            else if(myMinigunner.State == "MOVING")
            {
                gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
            }
            else if(myMinigunner.State == "ATTACKING")
            {
                gameSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.SHOOTING_UP);
            }
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(myMinigunner.health <= 0)
            {
                return;
            }

            Vector2 minigunnerPlottedPosition = new Vector2();
            minigunnerPlottedPosition.X = (float)Math.Round(myMinigunner.position.X);
            minigunnerPlottedPosition.Y = (float)Math.Round(myMinigunner.position.Y);


            //            gameSprite.Update(gameTime);
            //            animationSequence.Update();
            //int currentFrame = animationSequence.GetCurrentFrame();
            //Texture2D currentTexture = textureList[(int)currentFrame];
            //spriteBatch.Draw(currentTexture, minigunnerPlottedPosition, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);


            //if(drawBoundingRectangle)
            //{
            //    spriteBatch.Draw(spriteBorderRectangleTexture, minigunnerPlottedPosition, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);

            //}

            gameSprite.Draw(gameTime, spriteBatch, minigunnerPlottedPosition);

            if (myMinigunner.selected)
            {
                unitSelectionCursor.Draw(gameTime, spriteBatch);
            }


        }

        public void SetAnimate(bool animateFlag)
        {
            gameSprite.SetAnimate(animateFlag);
        }



    }
}
