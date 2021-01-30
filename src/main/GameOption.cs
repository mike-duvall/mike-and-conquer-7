



namespace mike_and_conquer.main

{
    public class GameOptions
    {

        public bool DRAW_TERRAIN_BORDER = false;
        public bool DRAW_BLOCKING_TERRAIN_BORDER = false;

        public bool IS_FULL_SCREEN = true;
//        public bool IS_FULL_SCREEN = false;

        public bool DRAW_SHROUD = true;
        // public bool DRAW_SHROUD = false;

        public float MAP_ZOOM = 1.0f;
        // public float MAP_ZOOM = 3.0f;

        //        public  bool PLAY_MUSIC = true;
        public bool PLAY_MUSIC = false;

        public int GAME_SPEED_DELAY_DIVISOR = 50;
        // public int GAME_SPEED_DELAY_DIVISOR = 200;
        // public int GAME_SPEED_DELAY_DIVISOR = 20;
        // public int GAME_SPEED_DELAY_DIVISOR = 1;

        public static GameOptions instance;

        public GameOptions()
        {
            GameOptions.instance = this;
        }


        public  void ToggleDrawTerrainBorder()
        {
            DRAW_TERRAIN_BORDER = !DRAW_TERRAIN_BORDER;
        }

        public  void ToggleDrawBlockingTerrainBorder()
        {
            DRAW_BLOCKING_TERRAIN_BORDER = !DRAW_BLOCKING_TERRAIN_BORDER;
        }

        public  void ToggleDrawShroud()
        {
            DRAW_SHROUD = !DRAW_SHROUD;
        }



    }
}

