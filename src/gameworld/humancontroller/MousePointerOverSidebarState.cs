﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.main;

namespace mike_and_conquer.gameworld.humancontroller
{
    class MousePointerOverSidebarState : HumanControllerState
    {
        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {
            MikeAndConquerGame.instance.log.Information("MousePointerOverSidebarState.Update() begin");

            if (MouseInputUtil.IsOverSidebar(newMouseState))
            {

                if (MouseInputUtil.LeftMouseButtonClicked(newMouseState, oldMouseState))
                {
                    Point mousePoint = newMouseState.Position;
                    Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);

                    Vector2 sidebarLocation = GameWorldView.instance.ConvertScreenLocationToSidebarLocation(mouseScreenLocation);

                    // TODO:  Add Sidebar class, have build buttons sit inside of it, iterate through
                    // them and ask if they contain point where sidebar was clicked
                    if (sidebarLocation.X > 0 && sidebarLocation.X < 64 && sidebarLocation.Y > 0 && sidebarLocation.Y < 48)
                    {
                        HandleClickOnBuildBarracks();
                    }
                    else if (sidebarLocation.X > 80 && sidebarLocation.X < 144 && sidebarLocation.Y > 0 && sidebarLocation.Y < 48)
                    {
                        HandleClickOnBuildMinigunner();
                    }

                }

                return this;
            }
            else
            {
                // TODO:  Check if units are selected or not first
                // to find correct state
                return new NeutralMapstate();
            }
        }


        private void HandleClickOnBuildBarracks()
        {
            GDIConstructionYard gdiConstructionYard = GameWorld.instance.GDIConstructionYard;

            if (gdiConstructionYard.IsBarracksReadyToPlace)
            {
                gdiConstructionYard.CreateBarracksFromConstructionYard();
            }
            else if (!gdiConstructionYard.IsBuildingBarracks)
            {
                GameWorld.instance.GDIConstructionYard.StartBuildingBarracks();
            }

        }

        private void HandleClickOnBuildMinigunner()
        {
            GameWorld.instance.GDIBarracks.StartBuildingMinigunner();
        }


    }
}
