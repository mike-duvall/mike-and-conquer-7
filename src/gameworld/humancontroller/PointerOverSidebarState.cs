using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.main;

namespace mike_and_conquer.gameworld.humancontroller
{
    class PointerOverSidebarState : HumanControllerState
    {
        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {

            if (MouseInputUtil.IsOverSidebar(newMouseState))
            {

                if (MouseInputUtil.LeftMouseButtonClicked(newMouseState, oldMouseState))
                {
                    Point sidebarWorldLocation = MouseInputUtil.GetSidebarWorldLocationPointFromMouseState(newMouseState);

                    // TODO:  Add Sidebar class, have build buttons sit inside of it, iterate through
                    // them and ask if they contain point where sidebar was clicked
                    if (sidebarWorldLocation.X > 0 && sidebarWorldLocation.X < 64 && sidebarWorldLocation.Y > 0 && sidebarWorldLocation.Y < 48)
                    {
                        return HandleClickOnBuildBarracks();
                    }
                    else if (sidebarWorldLocation.X > 80 && sidebarWorldLocation.X < 144 && sidebarWorldLocation.Y > 0 && sidebarWorldLocation.Y < 48)
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
                return new PointerOverMapState();
            }
        }


        private HumanControllerState HandleClickOnBuildBarracks()
        {
            GDIConstructionYard gdiConstructionYard = GameWorld.instance.GDIConstructionYard;

            if (gdiConstructionYard.IsBarracksReadyToPlace)
            {
                //gdiConstructionYard.CreateBarracksFromConstructionYard();
                return new PlacingBuldingState();
            }
            else if (!gdiConstructionYard.IsBuildingBarracks)
            {
                GameWorld.instance.GDIConstructionYard.StartBuildingBarracks();
            }

            return this;


        }

        private void HandleClickOnBuildMinigunner()
        {
            GameWorld.instance.GDIBarracks.StartBuildingMinigunner();
        }


    }
}
