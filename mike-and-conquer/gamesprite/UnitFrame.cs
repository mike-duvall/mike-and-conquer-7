
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace mike_and_conquer.gamesprite
{
    public class UnitFrame
    {
        private Texture2D texture;
        private byte[] frameData;
        private List<int> shadowIndexList;


        public Texture2D Texture
        {
            get { return texture; }
        }

        public List<int> ShadowIndexList
        {
            get { return shadowIndexList; }
        }

        public byte[] FrameData
        {
            get { return frameData; }
        }


        public UnitFrame(Texture2D texture, byte[] frameData, List<int> shadowIndexList)
        {
            this.texture = texture;
            this.frameData = frameData;
            this.shadowIndexList = shadowIndexList;
        }
    }
}
