
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;


namespace mike_and_conquer.gameview
{
    public class GDIConstructionYardView
    {

        // TODO:  Consider something other than UnitSprite in future
        private UnitSprite unitSprite;

        private GDIConstructionYard myGdiConstructionYard;

        public const string SPRITE_KEY = "ConstructionYard";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "Shp\\fact.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public GDIConstructionYardView(GDIConstructionYard constructionYard)
        {
            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSprite.drawShadow = true;
            this.unitSprite.drawBoundingRectangle = false;
            this.unitSprite.middleOfSpriteInSpriteCoordinates.Y += (GameWorld.MAP_TILE_HEIGHT / 2);
            this.myGdiConstructionYard = constructionYard;

            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence(0, animationSequence);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            unitSprite.Draw(gameTime, spriteBatch, myGdiConstructionYard.positionInWorldCoordinates,
                SpriteSortLayers.BUILDING_DEPTH);
        }

        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {

            unitSprite.DrawNoShadow(gameTime, spriteBatch, myGdiConstructionYard.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);

        }



        // internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        // {
        //     if (myMCV.health <= 0)
        //     {
        //         return;
        //     }
        //
        //     mcvSelectionBox.position = new Vector2(myMCV.positionInWorldCoordinates.X, myMCV.positionInWorldCoordinates.Y);
        //
        //     unitSprite.DrawNoShadow(gameTime, spriteBatch, myMCV.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);
        //
        //     if (myMCV.selected)
        //     {
        //         mcvSelectionBox.Draw(gameTime, spriteBatch);
        //     }
        //
        // }



    }
}
