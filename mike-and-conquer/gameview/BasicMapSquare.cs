

using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

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
            this.gameSprite.drawBoundingRectangle = true;
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


        public bool ContainsPoint(Point aPoint)
        {
            int height = 24;
            int width = 24;
            int leftX = (int) position.X - (width / 2);
            int topY = (int) position.Y - (height / 2);
            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }

        public Point GetCenter()
        {
            return new Point((int) position.X, (int) position.Y);
        }
    }
}
