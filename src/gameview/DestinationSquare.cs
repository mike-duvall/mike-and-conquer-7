
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    class DestinationSquare
    {

        public const string SPRITE_KEY = "DestinationSquare";

        private SingleTextureSprite sprite;
        public Vector2 position;

        public DestinationSquare()
        {
            sprite = new SingleTextureSprite(MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(SPRITE_KEY));
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float layerDepth = 1.0f;
            sprite.Draw(gameTime, spriteBatch, position, 1.0f);
        }
    }
}
