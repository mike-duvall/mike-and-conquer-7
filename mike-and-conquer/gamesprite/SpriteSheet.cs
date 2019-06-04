
using System.Collections.Generic;
//using System.Linq;
using OpenRA.Graphics;

namespace mike_and_conquer.gamesprite
{
    class SpriteSheet
    {

        private Dictionary<string, List<UnitFrame>> unitFrameMap;

//
//
//        public void LoadUnitFramesFromSpriteFrames(OpenRA.IReadOnlyList<ISpriteFrame> spriteFrameList)
//        {
//
//            Pickup here
//            // Convert this code to work here
//            // May need to pull other bits from TextureListMap.LoadAllTexturesFromShpFile()
//            // to do color mapping
//
//            foreach (ISpriteFrame frame in spriteFrameList)
//            {
//                byte[] frameData = frame.Data;
//                List<int> shadowIndexList = new List<int>();
//
//                Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, shpTDSprite.Size.Width, shpTDSprite.Size.Height);
//                int numPixels = texture2D.Width * texture2D.Height;
//                Color[] texturePixelData = new Color[numPixels];
//
//                for (int i = 0; i < numPixels; i++)
//                {
//                    int basePaletteIndex = frameData[i];
//                    int mappedPaletteIndex = shpFileColorMapper.MapColorIndex(basePaletteIndex);
//                    uint mappedColor = palette[mappedPaletteIndex];
//
//                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
//                    Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
//                    texturePixelData[i] = xnaColor;
//
//                    if (mappedPaletteIndex == 4)
//                    {
//                        shadowIndexList.Add(i);
//                    }
//                }
//
//                texture2D.SetData(texturePixelData);
//                shpStream.Close();
//                ShpFileImage shpFileImage = new ShpFileImage(texture2D, shadowIndexList, frameData);
//                spriteTextureList.shpFileImageList.Add(shpFileImage);
//            }
//
//
//        }
    }
}
