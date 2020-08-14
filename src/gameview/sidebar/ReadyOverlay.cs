﻿using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview.sidebar
{
    public class ReadyOverlay
    {

        public const string SPRITE_KEY = "SideBar/ReadyOverlay";

        private SingleTextureSprite sprite;
        public Vector2 position;



        public ReadyOverlay()
        {
            sprite = new SingleTextureSprite(MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(SPRITE_KEY));
            position = new Vector2(28,24);
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
//            float layerDepth = 1.0f;
            sprite.Draw(gameTime, spriteBatch, position, 1.0f);
        }
    }
}
