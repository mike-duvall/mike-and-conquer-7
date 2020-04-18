
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    class MCVSelectionBox
    {

        public const string SPRITE_KEY = "MCVSelectionBox";


        private SingleTextureSprite sprite;

        public Vector2 position { get; set; }

        public MCVSelectionBox()
        {
            sprite = new SingleTextureSprite(MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(SPRITE_KEY));
            sprite.middleOfSpriteInSpriteCoordinates.Y = sprite.middleOfSpriteInSpriteCoordinates.Y - 2;

        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float layerDepth = 1.0f;
            sprite.Draw(gameTime, spriteBatch, position, layerDepth);

        }
    }
}
