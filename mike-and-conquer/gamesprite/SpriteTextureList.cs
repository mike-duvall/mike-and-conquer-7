using System.Collections.Generic;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;


namespace mike_and_conquer
{
    public class SpriteTextureList
    {

        public List<Texture2D> textureList;
        public int textureWidth;
        public int textureHeight;
        public List<byte[]> frameDataList;

        public SpriteTextureList()
        {
            this.textureList = new List<Texture2D>();
            this.frameDataList = new List<byte[]>();
        }

    }
}
