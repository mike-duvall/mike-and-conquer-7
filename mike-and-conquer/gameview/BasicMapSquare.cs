
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Math = System.Math;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquare
    {
        private GameSprite gameSprite;


        Vector2 position;
        private int imageIndex;

        public  BasicMapSquare(int x, int y, string textureKey, int imageIndex )
        {
            this.position = new Vector2(x,y);
            this.gameSprite = new GameSprite(textureKey);
            this.imageIndex = imageIndex;
            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(imageIndex);
            gameSprite.AddAnimationSequence(0, animationSequence);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameSprite.Draw(gameTime, spriteBatch, this.position);
        }




    }
}
