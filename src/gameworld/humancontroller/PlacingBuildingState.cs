using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.main;

namespace mike_and_conquer.gameworld.humancontroller
{
    class PlacingBuldingState : HumanControllerState
    {
        //        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        //        {
        //
        //            if (MouseInputUtil.IsOverSidebar(newMouseState))
        //            {
        //
        //                if (MouseInputUtil.LeftMouseButtonClicked(newMouseState, oldMouseState))
        //                {
        //                    Point sidebarWorldLocation = MouseInputUtil.GetSidebarWorldLocationPointFromMouseState(newMouseState);
        //
        //                    // TODO:  Add Sidebar class, have build buttons sit inside of it, iterate through
        //                    // them and ask if they contain point where sidebar was clicked
        //                    if (sidebarWorldLocation.X > 0 && sidebarWorldLocation.X < 64 && sidebarWorldLocation.Y > 0 && sidebarWorldLocation.Y < 48)
        //                    {
        //                        HandleClickOnBuildBarracks();
        //                    }
        //                    else if (sidebarWorldLocation.X > 80 && sidebarWorldLocation.X < 144 && sidebarWorldLocation.Y > 0 && sidebarWorldLocation.Y < 48)
        //                    {
        //                        HandleClickOnBuildMinigunner();
        //                    }
        //
        //                }
        //
        //                return this;
        //            }
        //            else
        //            {
        //                // TODO:  Check if units are selected or not first
        //                // to find correct state
        //                return new PointerOverMapState();
        //            }
        //        }

        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {
            if (GameWorldView.instance.barracksPlacementIndicatorView == null)
            {
                GameWorldView.instance.barracksPlacementIndicatorView = new BarracksPlacementIndicatorView();
            }

            if (!MouseInputUtil.IsOverSidebar(newMouseState))
            {
                Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                GameWorldView.instance.barracksPlacementIndicatorView.position.X = mouseWorldLocationPoint.X;
                GameWorldView.instance.barracksPlacementIndicatorView.position.Y = mouseWorldLocationPoint.Y;

                if (MouseInputUtil.LeftMouseButtonUnclicked(newMouseState, oldMouseState))
                {
                    GDIConstructionYard gdiConstructionYard = GameWorld.instance.GDIConstructionYard;
                    gdiConstructionYard.CreateBarracksFromConstructionYard(mouseWorldLocationPoint.X, mouseWorldLocationPoint.Y);
                    GameWorldView.instance.barracksPlacementIndicatorView = null;
                    return new PointerOverMapState();
                }
            }
            return this;

        }

    }
}
