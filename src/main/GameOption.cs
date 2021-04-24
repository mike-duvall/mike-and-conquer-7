



namespace mike_and_conquer.main

{
    public class GameOptions
    {

        public bool DrawTerrainBorder = false;
        public bool DrawBlockingTerrainBorder = false;

        public bool IsFullScreen = true;
        // public bool IsFullScreen = false;

        public bool DrawShroud = true;
        // public bool DrawShroud = false;

        public float MapZoomLevel = 1.0f;
        // public float MapZoomLevel = 3.0f;

        //        public  bool PlayMusic = true;
        public bool PlayMusic = false;


        // public int GameSpeedDelayDivisor = 250;  // Slowest

        // public int GameSpeedDelayDivisor = 125;  // Slower

        // public int GameSpeedDelayDivisor = 85; // Slow

        public int GameSpeedDelayDivisor = 63; // Moderate

        // public int GameSpeedDelayDivisor = 43;  // Normal

        // public int GameSpeedDelayDivisor = 30;  // Fast

        // public int GameSpeedDelayDivisor = 25; // Faster

        // public int GameSpeedDelayDivisor = 23; // Fastest

        public static GameOptions instance;

        public GameOptions()
        {
            GameOptions.instance = this;
        }


        public  void ToggleDrawTerrainBorder()
        {
            DrawTerrainBorder = !DrawTerrainBorder;
        }

        public  void ToggleDrawBlockingTerrainBorder()
        {
            DrawBlockingTerrainBorder = !DrawBlockingTerrainBorder;
        }

        public  void ToggleDrawShroud()
        {
            DrawShroud = !DrawShroud;
        }



    }
}

