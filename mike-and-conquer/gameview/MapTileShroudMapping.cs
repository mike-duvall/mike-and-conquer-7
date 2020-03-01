
using System;

namespace mike_and_conquer.gameview
{
    public class MapTileShroudMapping
    {

        public MapTileInstance.MapTileVisibility east;
        public Nullable<MapTileInstance.MapTileVisibility> southEast;
        public MapTileInstance.MapTileVisibility south;
        public Nullable<MapTileInstance.MapTileVisibility> southWest;
        public MapTileInstance.MapTileVisibility west;
        public Nullable<MapTileInstance.MapTileVisibility> northWest;
        public MapTileInstance.MapTileVisibility north;
        public Nullable<MapTileInstance.MapTileVisibility> northEast;

        public int shroudTileIndex;
        
        // public MapTileShroudMapping(
        //     MapTileInstance.MapTileVisibility east,
        //     MapTileInstance.MapTileVisibility south,
        //     MapTileInstance.MapTileVisibility west,
        //     MapTileInstance.MapTileVisibility north,
        //     int shroudTileIndex)
        // {
        //     this.east = east;
        //     this.south = south;
        //     this.west = west;
        //     this.north = north;
        //     this.northEast = null;
        //     this.southEast = null;
        //     this.southWest = null;
        //     this.northWest = null;
        //     this.shroudTileIndex = shroudTileIndex;
        // }

        // public MapTileShroudMapping(
        //     MapTileInstance.MapTileVisibility east,
        //     MapTileInstance.MapTileVisibility south,
        //     MapTileInstance.MapTileVisibility west,
        //     MapTileInstance.MapTileVisibility north,
        //     MapTileInstance.MapTileVisibility northEast,
        //     MapTileInstance.MapTileVisibility southEast,
        //     int shroudTileIndex)
        // {
        //     this.east = east;
        //     this.south = south;
        //     this.west = west;
        //     this.north = north;
        //     this.northEast = northEast;
        //     this.southEast = southEast;
        //     this.southWest = null;
        //     this.northWest = null;
        //     this.shroudTileIndex = shroudTileIndex;
        // }


        public MapTileShroudMapping(
            MapTileInstance.MapTileVisibility east,
            MapTileInstance.MapTileVisibility southEast,
            MapTileInstance.MapTileVisibility south,
            MapTileInstance.MapTileVisibility southWest,
            MapTileInstance.MapTileVisibility west,
            MapTileInstance.MapTileVisibility northWest,
            MapTileInstance.MapTileVisibility north,
            MapTileInstance.MapTileVisibility northEast,

            int shroudTileIndex)
        {
            this.east = east;
            this.south = south;
            this.west = west;
            this.north = north;
            this.northEast = northEast;
            this.southEast = southEast;
            this.southWest = southWest;
            this.northWest = northWest;
            this.shroudTileIndex = shroudTileIndex;
        }





    }
}