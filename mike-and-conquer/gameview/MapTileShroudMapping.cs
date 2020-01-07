
using System;

namespace mike_and_conquer.gameview
{
    public class MapTileShroudMapping
    {
        public MapTileInstance.MapTileVisibility east;
        public MapTileInstance.MapTileVisibility south;
        public MapTileInstance.MapTileVisibility west;
        public MapTileInstance.MapTileVisibility north;

        public Nullable<MapTileInstance.MapTileVisibility> northEast;
        public Nullable<MapTileInstance.MapTileVisibility> southEast;

        public int shroudTileIndex;


        public MapTileShroudMapping(
            MapTileInstance.MapTileVisibility east,
            MapTileInstance.MapTileVisibility south,
            MapTileInstance.MapTileVisibility west,
            MapTileInstance.MapTileVisibility north,
            int shroudTileIndex)
        {
            this.east = east;
            this.south = south;
            this.west = west;
            this.north = north;
            this.northEast = null;
            this.southEast = null;
            this.shroudTileIndex = shroudTileIndex;
        }

        public MapTileShroudMapping(
            MapTileInstance.MapTileVisibility east,
            MapTileInstance.MapTileVisibility south,
            MapTileInstance.MapTileVisibility west,
            MapTileInstance.MapTileVisibility north,
            MapTileInstance.MapTileVisibility northEast,
            MapTileInstance.MapTileVisibility southEast,
            int shroudTileIndex)
        {
            this.east = east;
            this.south = south;
            this.west = west;
            this.north = north;
            this.northEast = northEast;
            this.southEast = southEast;
            this.shroudTileIndex = shroudTileIndex;
        }





    }
}