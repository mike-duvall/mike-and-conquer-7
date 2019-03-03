

using System.Linq;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Collections.Generic;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquare
    {
        public GameSprite gameSprite;



        Vector2 positionInWorldCoordinates;
        private int imageIndex;
        private string textureKey;

        public  BasicMapSquare(int x, int y, string textureKey, int imageIndex )
        {
            this.positionInWorldCoordinates = new Vector2(x,y);
            this.gameSprite = new GameSprite(textureKey);
            this.gameSprite.drawBoundingRectangle = false;
            this.imageIndex = imageIndex;
            this.textureKey = textureKey;
            SetupAnimations();
        }


        internal int GetPaletteIndexOfCoordinate(int x, int y)
        {
            SpriteTextureList list = MikeAndConquerGame.instance.TextureListMap.GetTextureList(textureKey);
            ShpFileImage shpFileImage = list.shpFileImageList[imageIndex];

            int frameDataIndex = y * list.textureWidth + x;
            return shpFileImage.frameData[frameDataIndex];
        }

        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(imageIndex);
            gameSprite.AddAnimationSequence(0, animationSequence);
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameSprite.Draw(gameTime, spriteBatch, this.positionInWorldCoordinates);
        }


        public bool ContainsPoint(Point aPoint)
        {
            int height = 24;
            int width = 24;
            int leftX = (int) positionInWorldCoordinates.X - (width / 2);
            int topY = (int) positionInWorldCoordinates.Y - (height / 2);
            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }

        public Point GetCenter()
        {
            return new Point((int) positionInWorldCoordinates.X, (int) positionInWorldCoordinates.Y);
        }

        public bool IsBlockingTerrain()
        {
            Dictionary<string, int[]> blockingTerrainMap = MikeAndConquerGame.instance.gameMap.blockingTerrainMap;

            if (blockingTerrainMap.ContainsKey(textureKey))
            {
                int[] blockedImageIndexes = blockingTerrainMap[textureKey];
                if (blockedImageIndexes == null)
                {
                    return true;
                }
                else
                {
                    return blockedImageIndexes.Contains(imageIndex);
                }
            }
        
            return false;
        }

        internal int GetMapSquareX()
        {
            int mapSquareX = (int)((this.positionInWorldCoordinates.X - 12) / 24);
            return mapSquareX;
        }

        internal int GetMapSquareY()
        {
            int mapSquareY = (int)((this.positionInWorldCoordinates.Y - 12) / 24);
            return mapSquareY;

        }

    }
}
