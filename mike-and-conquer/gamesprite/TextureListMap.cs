
using System.Collections.Generic;

using ShpTDSprite = OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite;
using TmpTDLoader = OpenRA.Mods.Common.SpriteLoaders.TmpTDLoader;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;

using ISpriteFrame = OpenRA.Graphics.ISpriteFrame;

namespace mike_and_conquer
{
    public class TextureListMap
    {

        private Dictionary<string, SpriteTextureList> spriteTextureListMap;

        public const string CLEAR1_SHP = "Content\\clear1.tem";

        public const string D04_TEM = "Content\\d04.tem";
        public const string D09_TEM = "Content\\d09.tem";
        public const string D13_TEM = "Content\\d13.tem";
        public const string D15_TEM = "Content\\d15.tem";
        public const string D20_TEM = "Content\\d20.tem";
        public const string D21_TEM = "Content\\d21.tem";
        public const string D23_TEM = "Content\\d23.tem";

        public const string P07_TEM = "Content\\p07.tem";
        public const string P08_TEM = "Content\\p08.tem";

        public const string S09_TEM = "Content\\s09.tem";
        public const string S10_TEM = "Content\\s10.tem";
        public const string S11_TEM = "Content\\s11.tem";
        public const string S12_TEM = "Content\\s12.tem";
        public const string S14_TEM = "Content\\s14.tem";
        public const string S22_TEM = "Content\\s22.tem";
        public const string S29_TEM = "Content\\s29.tem";
        public const string S32_TEM = "Content\\s32.tem";
        public const string S34_TEM = "Content\\s34.tem";
        public const string S35_TEM = "Content\\s35.tem";

        public const string SH1_TEM = "Content\\sh1.tem";
        public const string SH2_TEM = "Content\\sh2.tem";
        public const string SH3_TEM = "Content\\sh3.tem";
        public const string SH4_TEM = "Content\\sh4.tem";
        public const string SH5_TEM = "Content\\sh5.tem";
        public const string SH6_TEM = "Content\\sh6.tem";
        public const string SH9_TEM = "Content\\sh9.tem";
        public const string SH10_TEM = "Content\\sh10.tem";
        public const string SH17_TEM = "Content\\sh17.tem";
        public const string SH18_TEM = "Content\\sh18.tem";

        public const string W1_TEM = "Content\\w1.tem";
        public const string W2_TEM = "Content\\w2.tem";

        public TextureListMap()
        {
            spriteTextureListMap = new Dictionary<string, SpriteTextureList>();
        }
        public void AddTextureList(string key, SpriteTextureList spriteTextureList)
        {
            spriteTextureListMap.Add(key, spriteTextureList);
        }

        public SpriteTextureList GetTextureList(string key)
        {
            if (spriteTextureListMap.ContainsKey(key))
            {
                return spriteTextureListMap[key];
            }
            else
            {
                return spriteTextureListMap[key];
            }
        }

        internal void LoadSpriteListFromShpFile(string key, string shpFileName, ShpFileColorMapper shpFileColorMapper)
        {
            AddTextureList(key, LoadAllTexturesFromShpFile(shpFileName, shpFileColorMapper));
        }

        internal void LoadSpriteListFromTmpFile( string shpFileName)
        {
            AddTextureList(shpFileName, LoadAllTexturesFromTmpFile(shpFileName));
        }

        // TODO Consider pulling loading of ShpFile to TextListMap into a separate ShpFileLoader class
        internal SpriteTextureList LoadAllTexturesFromShpFile(string shpFileName, ShpFileColorMapper shpFileColorMapper)
        {

            int[] remap = { };

            SpriteTextureList spriteTextureList = new SpriteTextureList();
            OpenRA.Graphics.ImmutablePalette palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);

            System.IO.FileStream shpStream = System.IO.File.Open(shpFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader loader = new OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader();
            ShpTDSprite shpTDSprite = new OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite(shpStream);

            int unscaledWidth = shpTDSprite.Frames[0].Size.Width;
            int unscaledHeight = shpTDSprite.Frames[0].Size.Height;

            spriteTextureList.textureWidth = unscaledWidth;
            spriteTextureList.textureHeight = unscaledHeight;

            foreach (OpenRA.Graphics.ISpriteFrame frame in shpTDSprite.Frames)
            {
                byte[] frameData = frame.Data;

                Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, shpTDSprite.Size.Width, shpTDSprite.Size.Height);
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
                }

                texture2D.SetData(texturePixelData);
                shpStream.Close();
                spriteTextureList.textureList.Add(texture2D);
            }
            return spriteTextureList;

        }

        internal SpriteTextureList LoadAllTexturesFromTmpFile(string fileName)
        {

            TmpTDLoader tmpTDLoader = new TmpTDLoader();
            System.IO.FileStream tmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);

            ISpriteFrame[] frames;
            tmpTDLoader.TryParseSprite(tmpStream, out frames );

            int[] remap = { };

            SpriteTextureList spriteTextureList = new SpriteTextureList();
            spriteTextureList.textureWidth = -1;
            spriteTextureList.textureHeight = -1;


            OpenRA.Graphics.ImmutablePalette palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);

            foreach (OpenRA.Graphics.ISpriteFrame frame in frames)
            {
                byte[] frameData = frame.Data;

                if( frameData.Length == 0)
                {
                    spriteTextureList.textureList.Add(null);
                    continue;
                }
                //                Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, spriteTextureList.textureWidth, spriteTextureList.textureHeight);
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
                tmpStream.Close();
                spriteTextureList.textureWidth = frame.Size.Width;
                spriteTextureList.textureHeight = frame.Size.Height;

                spriteTextureList.textureList.Add(texture2D);
            }

            return spriteTextureList;

        }



    }
}

