
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using System.Linq;
using OpenRA.Graphics;

namespace mike_and_conquer.gamesprite
{
    class SpriteSheet
    {

        private Dictionary<string, List<UnitFrame>> unitFrameMap;
        private Dictionary<string, List<MapTileFrame>> mapTileFrameMap;


        Pickup here:
        // Create MapTileFrame, as diagramsed.  Will have Texture2D and shadow Texture2D(maybe shadow Texture2d comes later?)
        // Add LoadMapTileFramesFromSpriteFrames method

        // Once that all works, then switch over existing code to use SpriteSheet instead of TextureListMap
        // May have to rework GameSprite, or create UnitSprite and MapTileSprite, or something like that

        public SpriteSheet()
        {
            unitFrameMap = new Dictionary<string, List<UnitFrame>>();
        }

        public void LoadUnitFramesFromSpriteFrames(string shpFileName, OpenRA.IReadOnlyList<ISpriteFrame> spriteFrameList, ShpFileColorMapper shpFileColorMapper)
        {

            int[] remap = { };
            ImmutablePalette palette = new ImmutablePalette("Content\\temperat.pal", remap);
            List<UnitFrame> unitFrameList = new List<UnitFrame>();

            foreach (ISpriteFrame frame in spriteFrameList)
            {
                byte[] frameData = frame.Data;
                List<int> shadowIndexList = new List<int>();

                Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, frame.Size.Width, frame.Size.Height);
                int numPixels = texture2D.Width * texture2D.Height;
                Color[] texturePixelData = new Color[numPixels];

                for (int i = 0; i < numPixels; i++)
                {
                    int basePaletteIndex = frameData[i];
                    int mappedPaletteIndex = shpFileColorMapper.MapColorIndex(basePaletteIndex);
                    uint mappedColor = palette[mappedPaletteIndex];

                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                    Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                    texturePixelData[i] = xnaColor;

                    if (mappedPaletteIndex == 4)
                    {
                        shadowIndexList.Add(i);
                    }
                }

                texture2D.SetData(texturePixelData);
                UnitFrame unitFrame = new UnitFrame(texture2D, shadowIndexList);
                unitFrameList.Add(unitFrame);
            }

            unitFrameMap.Add(shpFileName, unitFrameList);

        }
    }
}
