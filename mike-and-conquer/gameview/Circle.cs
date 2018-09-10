
using GameTime = Microsoft.Xna.Framework.GameTime;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameobjects
{
    class Circle
    {

        public const string CIRCLE_SPRITE_KEY = "Circle";


        private GameSprite gameSprite;
        public Vector2 position;


        public Circle()
        {
            gameSprite = new GameSprite(CIRCLE_SPRITE_KEY);
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            gameSprite.AddAnimationSequence(0, animationSequence);
            gameSprite.SetCurrentAnimationSequenceIndex(0);

        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameSprite.Draw(gameTime, spriteBatch, position);
        }
    }
}
