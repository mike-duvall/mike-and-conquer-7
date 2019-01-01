using System.Collections.Generic;

using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Point = Microsoft.Xna.Framework.Point;


namespace mike_and_conquer
{
    public class SpriteTextureList
    {

        public List<Texture2D> textureList;
        public List<List<int>> shadowIndexLists;
        public int textureWidth;
        public int textureHeight;
        public List<byte[]> frameDataList;

        public SpriteTextureList()
        {
            this.textureList = new List<Texture2D>();
            this.frameDataList = new List<byte[]>();
            this.shadowIndexLists = new List<List<int>>();
        }

    }
}
