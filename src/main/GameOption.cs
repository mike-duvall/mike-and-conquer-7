



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


        public enum GameSpeed
        {
            Slowest = 250,
            Slower = 125,
            Slow = 85, 
            Moderate = 63,
            Normal = 40,
            Fast = 30,
            Faster = 25,
            Fastest = 24
        }



        public int GameSpeedDelayDivisor = (int) GameSpeed.Moderate; 

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

