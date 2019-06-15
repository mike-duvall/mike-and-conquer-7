
using System.Collections.Generic;
using mike_and_conquer.gamesprite;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;


using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquareView
    {
//        public GameSprite gameSprite;

        public MapTileSprite mapTileSprite;
        public BasicMapSquare myBasicMapSquare;

        private int imageIndex;
        private string textureKey;

        public BasicMapSquareView(BasicMapSquare aBasicMapSquare )
        {

            this.myBasicMapSquare = aBasicMapSquare;
            imageIndex = myBasicMapSquare.ImageIndex;
            textureKey = myBasicMapSquare.TextureKey;
            this.mapTileSprite = new MapTileSprite(textureKey);
            this.mapTileSprite.drawBoundingRectangle = false;

            SetupAnimations();
        }


        internal int GetPaletteIndexOfCoordinate(int x, int y)
        {

            // Pickup here
            // Update shadow mapping to not use TextureListMap, but to instead use SpriteSheet to get at the frameData
            // Possible code below:
//            List<MapTileFrame> mapTileFrameList = MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
//            MapTileFrame mapTileFrame = mapTileFrameList[imageIndex];
//            byte[] frameData = mapTileFrame.FrameData;

            SpriteTextureList list = MikeAndConquerGame.instance.TextureListMap.GetTextureList(textureKey);
            ShpFileImage shpFileImage = list.shpFileImageList[imageIndex];

            int frameDataIndex = y * list.textureWidth + x;
            return shpFileImage.frameData[frameDataIndex];
        }

        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(imageIndex);
            mapTileSprite.AddAnimationSequence(0, animationSequence);
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            mapTileSprite.Draw(gameTime, spriteBatch, this.myBasicMapSquare.PositionInWorldCoordinates);
        }


    }
}
