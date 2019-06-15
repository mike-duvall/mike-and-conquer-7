
using Microsoft.Xna.Framework.Graphics;

namespace mike_and_conquer.gamesprite
{
    public class MapTileFrame
    {

        private Texture2D mapTileTexture2D;
        private byte[] frameData;

        public Texture2D Texture
        {
            get { return mapTileTexture2D; }
        }

        public byte[] FrameData
        {
            get { return frameData; }
        }

        public MapTileFrame(Texture2D mapTileTexture2D, byte[] frameData)
        {
            this.mapTileTexture2D = mapTileTexture2D;
            this.frameData = frameData;
        }

    }
}
