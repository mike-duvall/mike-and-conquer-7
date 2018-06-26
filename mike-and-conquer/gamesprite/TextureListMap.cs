
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

        public const string CLEAR1_SHP_FILE_NAME = "Content\\clear1.tem";
        public const string S12_TEM = "Content\\s12.tem";

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
            return spriteTextureListMap[key];
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

                Texture2D texture2D = new Texture2D(MikeAndConqueryGame.instance.GraphicsDevice, shpTDSprite.Size.Width, shpTDSprite.Size.Height);
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

            //public bool TryParseSprite(Stream s, out ISpriteFrame[] frames)

            System.IO.FileStream tmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);

            ISpriteFrame[] frames;
            tmpTDLoader.TryParseSprite(tmpStream, out frames );

            int[] remap = { };

            SpriteTextureList spriteTextureList = new SpriteTextureList();
            OpenRA.Graphics.ImmutablePalette palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);

            int unscaledWidth = frames[0].Size.Width;
            int unscaledHeight = frames[0].Size.Height;

            spriteTextureList.textureWidth = unscaledWidth;
            spriteTextureList.textureHeight = unscaledHeight;

            foreach (OpenRA.Graphics.ISpriteFrame frame in frames)
            {
                byte[] frameData = frame.Data;

                Texture2D texture2D = new Texture2D(MikeAndConqueryGame.instance.GraphicsDevice, spriteTextureList.textureWidth, spriteTextureList.textureHeight);
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
                spriteTextureList.textureList.Add(texture2D);
            }
            return spriteTextureList;

        }



    }
}
