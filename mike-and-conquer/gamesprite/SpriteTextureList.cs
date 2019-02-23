using System.Collections.Generic;

using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;


namespace mike_and_conquer
{
    public class SpriteTextureList
    {

        public int textureWidth;
        public int textureHeight;
        public List<ShpFileImage> shpFileImageList;

        public SpriteTextureList()
        {
            this.shpFileImageList = new List<ShpFileImage>();
        }

    }
}
