


namespace mike_and_conquer
{


    public class MapTileType
    {
        private string textureKey;
        private byte imageIndex;
        private bool isBlockingTerrain;


        public string TextureKey
        {
            get { return textureKey; }
        }

        public byte ImageIndex
        {
            get { return imageIndex; }
        }

        public bool IsBlockingTerrain
        {
            get { return isBlockingTerrain; }
        }


        public MapTileType(string textureKey, byte imageIndex, bool isBlockingTerrain)
        {
            this.textureKey = textureKey;
            this.imageIndex = imageIndex;
            this.isBlockingTerrain = isBlockingTerrain;
        }

    }
}
