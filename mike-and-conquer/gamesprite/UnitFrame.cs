
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace mike_and_conquer.gamesprite
{
    class UnitFrame
    {
        private Texture2D texture;
        private List<int> shadowIndexList;

        public UnitFrame(Texture2D texture, List<int> shadowIndexList)
        {
            this.texture = texture;
            this.shadowIndexList = shadowIndexList;
        }
    }
}
