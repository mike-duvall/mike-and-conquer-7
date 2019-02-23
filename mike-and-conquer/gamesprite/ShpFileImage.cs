using System.Collections.Generic;

using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;


namespace mike_and_conquer
{
    public class ShpFileImage
    {

        public Texture2D texture;
        public List<int> shadowIndexList;
        public byte[] frameData;

        public ShpFileImage(Texture2D texture, List<int> shadowIndexList, byte[] frameData)
        {
            this.texture = texture;
            this.shadowIndexList = shadowIndexList;
            this.frameData = frameData;
        }

    }
}
