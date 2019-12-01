
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gameview;

namespace mike_and_conquer
{
    class PartiallyVisibileMapTileMask
    {
        private static Texture2D partiallyVisibleMask = null;


        public const string SPRITE_KEY = "MapTileShadow";
        public const string SHP_FILE_NAME = "shadow.shp";

        public static Texture2D PartiallyVisibleMask
        {
            get
            {
                if (partiallyVisibleMask == null)
                {
                    InitializePartiallyVisiblyMask();
                }
                return partiallyVisibleMask;
            }
        }

        private static void InitializePartiallyVisiblyMask()
        {
            List<UnitFrame> shadowFrameList =
                MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY);

            UnitFrame unitFrame = shadowFrameList[3];
            partiallyVisibleMask = unitFrame.Texture;

            int numPixels = partiallyVisibleMask.Width *
                            partiallyVisibleMask.Height;
            Color[] textureData = new Color[numPixels];
            partiallyVisibleMask.GetData(textureData);
            for (int i = 0; i < numPixels; i++)
            {
                Color color = textureData[i];
                if (color.R == 0 && color.G == 0 && color.B == 0)
                {
                    color.R = 1;
                    color.G = 2;
                    color.B = 3;
                    color.A = 255;
                    textureData[i] = color;
                }
            }

            partiallyVisibleMask.SetData(textureData);
        }
    }
}
