
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gamesprite;


namespace mike_and_conquer
{
    class PartiallyVisibileMapTileMask
    {

        public const string SPRITE_KEY = "MapTileShadow";
        public const string SHP_FILE_NAME = "shadow.shp";

        private static List<UnitFrame> shadowFrameList;


        public PartiallyVisibileMapTileMask()
        {
            if (shadowFrameList == null)
            {
                shadowFrameList =
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY);
            }
        }


        public Texture2D GetMask(int index)
        {
            return shadowFrameList[index].Texture;
        }
    }
}
