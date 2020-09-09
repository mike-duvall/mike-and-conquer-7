using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class SandbagView
    {

        // TODO:  Using UnitSprite for now.  May need new Sprite
        // unique to sandbag like things, things like trees, etc
        private UnitSprite unitSprite;
        private Sandbag mySandbag;

        public const string SPRITE_KEY = "Sandbag";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "sbag.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public SandbagView(Sandbag sandbag)
        {
            this.mySandbag = sandbag;
            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSprite.drawShadow = true;
            SetupAnimations();
        }

        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(mySandbag.SandbagType);
            unitSprite.AddAnimationSequence(0, animationSequence);
        }

        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {
            unitSprite.DrawShadowOnly(gameTime, spriteBatch, mySandbag.MapTileLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            unitSprite.DrawNoShadow(gameTime, spriteBatch, mySandbag.MapTileLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
        }


    }
}
