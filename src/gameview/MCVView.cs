﻿using mike_and_conquer.gameobjects;
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
        // private UnitSelectionCursor unitSelectionCursor;
        private UnitSelectionCursor2 unitSelectionCursor;
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
            // this.mcvSelectionBox = new MCVSelectionBox();
            // this.unitSelectionCursor = new UnitSelectionCursor(myMCV, (int)this.myMCV.GameWorldLocation.WorldCoordinatesAsVector2.X, (int)this.myMCV.GameWorldLocation.WorldCoordinatesAsVector2.Y);
            this.unitSelectionCursor = new UnitSelectionCursor2(myMCV, (int)this.myMCV.GameWorldLocation.X, (int)this.myMCV.GameWorldLocation.Y);

            this.destinationSquare = new DestinationSquare();
            this.drawDestinationSquare = false;

            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence(0, animationSequence);

        }


        public void Update(GameTime gameTime)
        {
            unitSelectionCursor.Update(gameTime);
        }


        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (myMCV.Health <= 0)
            {
                return;
            }

            // unitSprite.DrawNoShadow(gameTime, spriteBatch, myMCV.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
            unitSprite.DrawNoShadow(gameTime, spriteBatch, myMCV.GameWorldLocation.AsVector2, SpriteSortLayers.UNIT_DEPTH);

            if (myMCV.selected)
            {
                unitSelectionCursor.DrawNoShadow(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }


        }


        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (myMCV.Health <= 0)
            {
                return;
            }

            // unitSprite.DrawShadowOnly(gameTime, spriteBatch, myMCV.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
            unitSprite.DrawShadowOnly(gameTime, spriteBatch, myMCV.GameWorldLocation.AsVector2, SpriteSortLayers.UNIT_DEPTH);

            if (myMCV.selected)
            {
                unitSelectionCursor.DrawShadowOnly(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }

        }



        public void SetAnimate(bool animateFlag)
        {
            unitSprite.SetAnimate(animateFlag);
        }



    }
}
