

using System;

namespace mike_and_conquer

{
    public class GameOptions
    {


        public static bool DRAW_TERRAIN_BORDER = false;
        public static bool DRAW_BLOCKING_TERRAIN_BORDER = false;
        public static bool IS_FULL_SCREEN = true;
//        public static bool IS_FULL_SCREEN = false;

        public static void ToggleDrawTerrainBorder()
        {
            DRAW_TERRAIN_BORDER = !DRAW_TERRAIN_BORDER;
        }

        public static void ToggleDrawBlockingTerrainBorder()
        {
            DRAW_BLOCKING_TERRAIN_BORDER = !DRAW_BLOCKING_TERRAIN_BORDER;
        }

    }
}
