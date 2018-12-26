

using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquare
    {
        public GameSprite gameSprite;



        Vector2 position;
        private int imageIndex;
        private string textureKey;

        public  BasicMapSquare(int x, int y, string textureKey, int imageIndex )
        {
            this.position = new Vector2(x,y);
            this.gameSprite = new GameSprite(textureKey);
            this.gameSprite.drawBoundingRectangle = false;
            this.imageIndex = imageIndex;
            this.textureKey = textureKey;
            SetupAnimations();
        }

        public int GetPaletteIndexOfUpperLeft()
        {
            SpriteTextureList list = MikeAndConquerGame.instance.TextureListMap.GetTextureList(textureKey);
            return list.frameDataList[imageIndex][0];
        }


        internal int GetPaletteIndexOfCoordinate(int mouseX, int mouseY)
        {
            SpriteTextureList list = MikeAndConquerGame.instance.TextureListMap.GetTextureList(textureKey);
            int index = mouseY * 24 + mouseX;
            return list.frameDataList[imageIndex][index];

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
