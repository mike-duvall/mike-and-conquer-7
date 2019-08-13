
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class TerrainView
    {


        // TODO:  Using UnitSprite for now.  May need new Sprite
        // unique to sandbag like things, things like trees, etc
        private UnitSprite unitSprite;


//        public const string SPRITE_KEY = "TC01";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
//        public const string SHP_FILE_NAME = "Content\\TC01.tem";


        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();
        private Point position;


        //        public TerrainView(Point position, string spriteKey)
        //        {
        //            this.unitSprite = new UnitSprite(spriteKey);
        //            this.unitSprite.middleOfSpriteInSpriteCoordinates = new Vector2(0,0);
        //            this.unitSprite.drawShadow = true;
        //            this.position = position;
        //
        //            SetupAnimations();
        //        }
        public TerrainView(TerrainItem terrainItem)
        {
            this.unitSprite = new UnitSprite( terrainItem.TerrainItemType);
            this.unitSprite.middleOfSpriteInSpriteCoordinates = new Vector2(0, 0);
            this.unitSprite.drawShadow = true;
            this.position = new Point((int) terrainItem.positionInWorldCoordinates.X,
                (int) terrainItem.positionInWorldCoordinates.Y);

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
            Vector2 positionAsVector2 = new Vector2(position.X, position.Y);
            unitSprite.Draw(gameTime, spriteBatch, positionAsVector2);
        }



    }
}
