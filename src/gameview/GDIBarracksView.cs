using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;


namespace mike_and_conquer.gameview
{
    public class GDIBarracksView
    {

        // TODO:  Consider something other than UnitSprite in future
        private UnitSprite unitSprite;

        private GDIBarracks myBarracks;

        public const string SPRITE_KEY = "Barracks";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "pyle.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public GDIBarracksView(GDIBarracks barracks)
        {
            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSprite.drawShadow = true;
            this.myBarracks = barracks;
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
            unitSprite.Draw(gameTime, spriteBatch, myBarracks.positionInWorldCoordinates,
                SpriteSortLayers.BUILDING_DEPTH);
        }

        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            unitSprite.DrawNoShadow(gameTime, spriteBatch, myBarracks.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);
        }

        internal void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {
            unitSprite.DrawShadowOnly(gameTime, spriteBatch, myBarracks.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);
        }


    }
}
