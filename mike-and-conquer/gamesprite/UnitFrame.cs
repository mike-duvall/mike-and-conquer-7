
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace mike_and_conquer.gamesprite
{
    public class UnitFrame
    {
        private Texture2D texture;
        private List<int> shadowIndexList;

        public Texture2D Texture
        {
            get { return texture; }
        }

        public List<int> ShadowIndexList
        {
            get { return shadowIndexList; }
        }


        public UnitFrame(Texture2D texture, List<int> shadowIndexList)
        {
            this.texture = texture;
            this.shadowIndexList = shadowIndexList;
        }
    }
}
