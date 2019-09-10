
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using System;

namespace mike_and_conquer.gameview
{
    public class TerrainView
    {


        // TODO:  Using UnitSprite for now.  May need new Sprite
        // unique to sandbag like things, things like trees, etc
        private TerrainSprite terrainSprite;


//        public const string SPRITE_KEY = "TC01";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
//        public const string SHP_FILE_NAME = "Content\\TC01.tem";


        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();
        private Point position;

//        internal void Update()
//        {
//            this.terrainSprite.Update();
//        }


        public TerrainView(TerrainItem terrainItem)
        {
            this.position = new Point((int)terrainItem.positionInWorldCoordinates.X,
                (int)terrainItem.positionInWorldCoordinates.Y);

            this.terrainSprite = new TerrainSprite( terrainItem.TerrainItemType, this.position);
            this.terrainSprite.middleOfSpriteInSpriteCoordinates = new Vector2(0, 0);
//            this.unitSprite.drawShadow = true;
            this.position = new Point((int) terrainItem.positionInWorldCoordinates.X,
                (int) terrainItem.positionInWorldCoordinates.Y);

//            SetupAnimations();
        }


//        private void SetupAnimations()
//        {
//            AnimationSequence animationSequence = new AnimationSequence(1);
//            animationSequence.AddFrame(0);
//            terrainSprite.AddAnimationSequence(0, animationSequence);
//        }


//        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
//        {
//            Vector2 positionAsVector2 = new Vector2(position.X, position.Y);
//            terrainSprite.Draw(gameTime, spriteBatch, positionAsVector2);
//        }


        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 positionAsVector2 = new Vector2(position.X, position.Y);
            this.terrainSprite.DrawShadowOnly(gameTime, spriteBatch, positionAsVector2);
        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 positionAsVector2 = new Vector2(position.X, position.Y);
            this.terrainSprite.DrawNoShadow(gameTime, spriteBatch, positionAsVector2);
        }
    }
}
