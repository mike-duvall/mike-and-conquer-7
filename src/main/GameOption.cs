﻿



using System;

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
            Fastest = 23
        }

        public static float ConvertGameSpeedToDelayDivisor(GameSpeed gameSpeed)
        {
            switch (gameSpeed)
            {
               // case GameSpeed.Slowest:
               //  return 250.0f;  4534
               // case GameSpeed.Slowest:
               //     return 251.0f;  //  4550
                // case GameSpeed.Slowest:
                //    return 249.0f;  // 4514
               // case GameSpeed.Slowest:
               //     return 248.0f;  // 4482

               case GameSpeed.Slowest:
                   return 248.5f;   // 4500

                case GameSpeed.Slower:
                   return 125;
               case GameSpeed.Slow:
                   return 85;
               case GameSpeed.Moderate:
                   return 63.0f;
                case GameSpeed.Normal:
                    return 40.0f;  

                case GameSpeed.Fast:
                   return 30.0f;
               case GameSpeed.Faster:
                   return 25.0f;
                // case GameSpeed.Fastest:
                //     return 23.6f; // 450
                // case GameSpeed.Fastest:
                //     return 23.0f; //434
                // case GameSpeed.Fastest:
                //     return 22.5f; // 433
                // case GameSpeed.Fastest:
                //     return 22.0f; // 415
                // case GameSpeed.Fastest:
                //     return 22.2f; // reloadTime(in code)=0.39960001373291015   //  measured reloadTime(in test): 416


                // case GameSpeed.Fastest:
                //     return 22.22f; // reloadTime(in code)= 0.399959987640381  //  measured reloadTime(in test): 416 (but large variation, 432 to 415)

               // case GameSpeed.Fastest:
               //     return 22.225f; // reloadTime(in code)= 0.400050006866455  //  measured reloadTime(in test):  433



                // case GameSpeed.Fastest:
                //     return 22.23f; // reloadTime(in code)= 0.400139991760254  //  measured reloadTime(in test): 433



                // case GameSpeed.Fastest:
                //    return 22.25f; //  reloadTime(in code)=0.4005 // measured reloadTime(in test): 433


                case GameSpeed.Fastest:
                    return 22.3f;  // reloadTime(in code)=0.40139998626709 // // measured reloadTime(in test): 433


                default:
                   throw new Exception("Unknown GameSpeed type");
            }
        }

        public GameSpeed CurrentGameSpeed = GameSpeed.Moderate;

        public float CurrentGameSpeedDivisor()
        {
            return ConvertGameSpeedToDelayDivisor(this.CurrentGameSpeed);
        }

        // public float GameSpeedDelayDivisor = GameOptions.ConvertGameSpeedToDelayDivisor(GameSpeed.Moderate);

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

