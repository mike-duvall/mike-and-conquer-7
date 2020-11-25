
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;

using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class Projectile120mmView
    {

        // TODO:  Using UnitSprite for now.  May need new Sprite
        // unique to sandbag like things, things like trees, etc
        private UnitSprite unitSprite;
        private Projectile120mm myProjectile120Mm;

        public int Id
        {
            get { return myProjectile120Mm.id; }
        }

        public const string SPRITE_KEY = "Projectile120mm";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "Shp\\120mm.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new NodShpFileColorMapper();
//        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new NodSecondaryShpFileColorMapper();


        public Projectile120mmView(Projectile120mm projectile120Mm)
        {
            this.myProjectile120Mm = projectile120Mm;
            this.unitSprite = new UnitSprite(SPRITE_KEY);
            SetupAnimations();
        }

        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);

            unitSprite.AddAnimationSequence(0, animationSequence);
            unitSprite.SetAnimate(false);
        }

//        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
//        {
//            unitSprite.DrawShadowOnly(gameTime, spriteBatch, myProjectile120Mm.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
//        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            unitSprite.DrawNoShadow(gameTime, spriteBatch, myProjectile120Mm.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
        }

//        private int previousFrameOffset = -1;
//        private int previousFrameStartTimeInMillis = -1;


    }
}
