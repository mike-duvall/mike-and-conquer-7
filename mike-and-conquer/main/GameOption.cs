

using System;

namespace mike_and_conquer

{
    public class GameOptions
    {
        public static bool DRAW_TERRAIN_BORDER = false;
        // Add fullScreen here, and screen size too

        public static void ToggleDrawTerrainBorder()
        {
            DRAW_TERRAIN_BORDER = !DRAW_TERRAIN_BORDER;
        }
    }
}
