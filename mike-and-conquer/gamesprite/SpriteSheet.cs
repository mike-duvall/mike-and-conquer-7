
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common.SpriteLoaders;

namespace mike_and_conquer.gamesprite
{
    public class SpriteSheet
    {

        private Dictionary<string, List<UnitFrame>> unitFrameMap;
        private Dictionary<string, List<MapTileFrame>> mapTileFrameMap;

        public SpriteSheet()
        {
            unitFrameMap = new Dictionary<string, List<UnitFrame>>();
            mapTileFrameMap = new Dictionary<string, List<MapTileFrame>>();
        }

        public List<UnitFrame> GetUnitFramesForShpFile(string shpFileName)
        {
            return unitFrameMap[shpFileName];
        }


        public List<MapTileFrame> GetMapTileFrameForTmpFile(string tmpFileName)
        {
            return mapTileFrameMap[tmpFileName];
        }

        public void LoadUnitFramesFromSpriteFrames(string spriteKey, OpenRA.IReadOnlyList<ISpriteFrame> spriteFrameList, ShpFileColorMapper shpFileColorMapper)
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

            unitFrameMap.Add(spriteKey, unitFrameList);

        }

        public void LoadMapTileFramesFromSpriteFrames(string tmpFileName, ISpriteFrame[] spriteFrameArray)
        {

            int[] remap = { };

            List<MapTileFrame> mapTileFrameList = new List<MapTileFrame>();

            ImmutablePalette palette = new ImmutablePalette("Content\\temperat.pal", remap);

            foreach (ISpriteFrame frame in spriteFrameArray)
            {
                byte[] frameData = frame.Data;

                if (frameData.Length == 0)
                {
                    //                    ShpFileImage emptyShpFileImage = new ShpFileImage(null, null, null);
                    //                    spriteTextureList.shpFileImageList.Add(emptyShpFileImage);
                    MapTileFrame nullMapTileFrame = new MapTileFrame(null, null);
                    mapTileFrameList.Add(nullMapTileFrame);
                    continue;
                }
                Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, frame.Size.Width, frame.Size.Height);
                int numPixels = texture2D.Width * texture2D.Height;
                Color[] texturePixelData = new Color[numPixels];

                for (int i = 0; i < numPixels; i++)
                {
                    int paletteIndex = frameData[i];
                    uint mappedColor = palette[paletteIndex];

                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                    Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                    texturePixelData[i] = xnaColor;
                }

                texture2D.SetData(texturePixelData);
                MapTileFrame mapTileFrame = new MapTileFrame(texture2D, frameData);
                mapTileFrameList.Add(mapTileFrame);
            }

            mapTileFrameMap.Add(tmpFileName, mapTileFrameList);


        }

    }
}
