

using System;

namespace mike_and_conquer

{
    public class GameOptions
    {


        public static bool DRAW_TERRAIN_BORDER = false;
        public static bool DRAW_BLOCKING_TERRAIN_BORDER = false;

        public static bool IS_FULL_SCREEN = true;
//        public static bool IS_FULL_SCREEN = false;

//        public static bool DRAW_SHROUD = true;
        public static bool DRAW_SHROUD = false;

        public static float INITIAL_MAP_ZOOM = 1.0f;
//        public static float INITIAL_MAP_ZOOM = 3.0f;


        public static void ToggleDrawTerrainBorder()
        {
            DRAW_TERRAIN_BORDER = !DRAW_TERRAIN_BORDER;
        }

        public static void ToggleDrawBlockingTerrainBorder()
        {
            DRAW_BLOCKING_TERRAIN_BORDER = !DRAW_BLOCKING_TERRAIN_BORDER;
        }

        public static void ToggleDrawShroud()
        {
            DRAW_SHROUD = !DRAW_SHROUD;
        }

    }
}
