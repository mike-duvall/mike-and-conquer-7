
using System.Collections.Generic;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;

namespace mike_and_conquer.util
{
    public class TextureUtil
    {
        public static Texture2D CopyTexture(Texture2D sourceTexture)
        {
            Color[] sourceTexturePixelData = new Color[sourceTexture.Width * sourceTexture.Height];
            sourceTexture.GetData(sourceTexturePixelData);

            Texture2D copyTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, sourceTexture.Width,
                sourceTexture.Height);

            Color[] copyTexturePixelData = new Color[copyTexture.Width * copyTexture.Height];
            copyTexture.GetData(copyTexturePixelData);

            int index = 0;
            foreach (Color color in sourceTexturePixelData)
            {
                copyTexturePixelData[index] = color;
                index++;
            }

            copyTexture.SetData(copyTexturePixelData);

            return copyTexture;
        }

        public static Texture2D UpdateShadowPixelsToTransparent(Texture2D texture, List<int> shadowIndexList)
        {
            Color[] texturePixelData = new Color[texture.Width * texture.Height];
            texture.GetData(texturePixelData);

            foreach (int shadowIndex in shadowIndexList)
            {
                texturePixelData[shadowIndex] = Color.Transparent;
            }

            texture.SetData(texturePixelData);

            return texture;
        }



    }
}
