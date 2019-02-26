using System.Collections.Generic;




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
