using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

 
namespace mike_and_conquer.gameview
{
    public class MCVView
    {
        private UnitSprite unitSprite;
        private MCVSelectionBox mcvSelectionBox;
        private DestinationSquare destinationSquare;
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
            this.mcvSelectionBox = new MCVSelectionBox();
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

            mcvSelectionBox.position = myMCV.GameWorldLocation.WorldCoordinatesAsVector2;


            unitSprite.Draw(gameTime, spriteBatch, myMCV.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);

            if (myMCV.selected)
            {
                mcvSelectionBox.Draw(gameTime, spriteBatch);
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

            mcvSelectionBox.position = new Vector2(myMCV.GameWorldLocation.WorldCoordinatesAsVector2.X, myMCV.GameWorldLocation.WorldCoordinatesAsVector2.Y);

            unitSprite.DrawNoShadow(gameTime, spriteBatch, myMCV.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);

            if (myMCV.selected)
            {
                mcvSelectionBox.Draw(gameTime, spriteBatch);
            }

        }


        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (myMCV.health <= 0)
            {
                return;
            }

            unitSprite.DrawShadowOnly(gameTime, spriteBatch, myMCV.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
        }



        public void SetAnimate(bool animateFlag)
        {
            unitSprite.SetAnimate(animateFlag);
        }



    }
}
