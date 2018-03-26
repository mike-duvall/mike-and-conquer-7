using System.Collections.Generic;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;


namespace mike_and_conquer
{
    class SpriteTextureList
    {

        public List<Texture2D> textureList;
        public int textureWidth;
        public int textureHeight;


        public SpriteTextureList()
        {
            this.textureList = new List<Texture2D>();
        }

    }
}
