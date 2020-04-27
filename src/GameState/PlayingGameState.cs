﻿using mike_and_conquer.gameobjects;

using mike_and_conquer.main;
using mike_and_conquer.src.inputhandler.windows;
using Boolean = System.Boolean;
using GameTime = Microsoft.Xna.Framework.GameTime;





namespace mike_and_conquer.gamestate
{
    class PlayingGameState : GameState
    {

        // private HumanPlayerController playerController;

        public PlayingGameState()
        {
            // playerController = new HumanPlayerController();;
        }

        public override string GetName()
        {
            return "Playing";
        }

        public override GameState Update(GameTime gameTime)
        {
            // TODO:  Consider pulling handling of GameEvents into base class
            GameState newGameState = GameWorld.instance.ProcessGameEvents();
            if (newGameState != null)
            {
                return newGameState;
            }

            GameWorld.instance.Update(gameTime);
            return DetermineNextGameState();
        }


        private GameState DetermineNextGameState()
        {
            if (NodMinigunnersExistAndAreAllDead())
            {
                return new MissionAccomplishedGameState();
            }

            if (GdiMinigunnersExistAndAreAllDead())
            {
                return new MissionFailedGameState();
            }
            else
            {
                return this;
            }
        }



        internal Boolean NodMinigunnersExistAndAreAllDead()
        {
            if(GameWorld.instance.NodMinigunnerList.Count == 0)
            {
                return false;
            }
            Boolean allDead = true;
            foreach (Minigunner nextMinigunner in GameWorld.instance.NodMinigunnerList)
            {
                if( nextMinigunner.health > 0)
                {
                    allDead = false;
                }
            }
            return allDead;
        }


        internal Boolean GdiMinigunnersExistAndAreAllDead()
        {
            if (GameWorld.instance.GDIMinigunnerList.Count == 0)
            {
                return false;
            }
            Boolean allDead = true;
            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    allDead = false;
                }
            }
            return allDead;
        }



    }
}
