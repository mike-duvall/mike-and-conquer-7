
using AnimationSequence = mike_and_conquer.util.AnimationSequence;


using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquareView
    {
        public GameSprite gameSprite;

        public BasicMapSquare myBasicMapSquare;

        private int imageIndex;
        private string textureKey;

        public BasicMapSquareView(BasicMapSquare aBasicMapSquare )
        {

            this.myBasicMapSquare = aBasicMapSquare;
            imageIndex = myBasicMapSquare.ImageIndex;
            textureKey = myBasicMapSquare.TextureKey;
            this.gameSprite = new GameSprite(textureKey);
            this.gameSprite.drawBoundingRectangle = false;

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
            gameSprite.Draw(gameTime, spriteBatch, this.myBasicMapSquare.PositionInWorldCoordinates);
        }


    }
}
