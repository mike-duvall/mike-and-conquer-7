
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

 
namespace mike_and_conquer.gameview
{
    public class MCVView
    {
        private UnitSprite unitSprite;
        private UnitSelectionCursor unitSelectionCursor;
        private DestinationSquare destinationSquare;
//        private Minigunner myMinigunner;
        private MCV myMCV;
        private bool drawDestinationSquare;

        //        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        public const string SPRITE_KEY = "MCV";
        public const string SHP_FILE_NAME = "Shp\\mcv.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();



        public MCVView(MCV mcv)
        {
            this.myMCV = mcv;
            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSprite.drawBoundingRectangle = false;
            this.unitSprite.drawShadow = true;
            this.unitSelectionCursor = new UnitSelectionCursor((int)this.myMCV.positionInWorldCoordinates.X, (int)this.myMCV.positionInWorldCoordinates.Y);
            this.destinationSquare = new DestinationSquare();
            this.drawDestinationSquare = false;

            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence(0, animationSequence);

        }



        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {


            if(myMCV.health <= 0)
            {
                return;
            }

            unitSelectionCursor.position = new Vector2(myMCV.positionInWorldCoordinates.X, myMCV.positionInWorldCoordinates.Y);

            unitSprite.Draw(gameTime, spriteBatch, myMCV.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);

            if (myMCV.selected)
            {
                unitSelectionCursor.Draw(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }

            if (this.drawDestinationSquare && this.myMCV.state == MCV.State.MOVING)
            {
                this.destinationSquare.position = this.myMCV.DestinationPosition;
                this.destinationSquare.Draw(gameTime, spriteBatch);
            }

        }

        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (myMCV.health <= 0)
            {
                return;
            }


            unitSelectionCursor.position = new Vector2(myMCV.positionInWorldCoordinates.X, myMCV.positionInWorldCoordinates.Y);


            unitSprite.DrawNoShadow(gameTime, spriteBatch, myMCV.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);

            if (myMCV.selected)
            {
                unitSelectionCursor.Draw(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }

        }


        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (myMCV.health <= 0)
            {
                return;
            }

            unitSprite.DrawShadowOnly(gameTime, spriteBatch, myMCV.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);
        }



        public void SetAnimate(bool animateFlag)
        {
            unitSprite.SetAnimate(animateFlag);
        }



    }
}
