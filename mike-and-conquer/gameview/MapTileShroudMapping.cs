
namespace mike_and_conquer.gameview
{
    public class MapTileShroudMapping
    {
        public MapTileInstance.MapTileVisibility right;
        public MapTileInstance.MapTileVisibility below;
        public MapTileInstance.MapTileVisibility left;
        public MapTileInstance.MapTileVisibility above;

        public int shroudTileIndex;


        public MapTileShroudMapping(
            MapTileInstance.MapTileVisibility right,
            MapTileInstance.MapTileVisibility below,
            MapTileInstance.MapTileVisibility left,
            MapTileInstance.MapTileVisibility above,
            int shroudTileIndex)
        {
            this.right = right;
            this.below = below;
            this.left = left;
            this.above = above;
            this.shroudTileIndex = shroudTileIndex;
        }




    }
}