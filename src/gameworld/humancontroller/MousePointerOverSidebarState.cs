using Microsoft.Xna.Framework;
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

            if (IsOverSidebar(newMouseState))
            {

                if (LeftMouseButtonClicked(newMouseState, oldMouseState))
                {
                    Point mousePoint = newMouseState.Position;
                    Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);

                    Vector2 sidebarLocation = GameWorldView.instance.ConvertScreenLocationToSidebarLocation(mouseScreenLocation);

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


        private bool IsOverSidebar(MouseState newMouseState)
        {
            Point mousePoint = newMouseState.Position;
            Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);

            Vector2 sidebarLocation = GameWorldView.instance.ConvertScreenLocationToSidebarLocation(mouseScreenLocation);

            if (sidebarLocation.X > 0 && sidebarLocation.Y > 0)
            {
                return true;
            }

            return false;

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

        private bool LeftMouseButtonClicked(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        }


        private void HandleClickOnBuildMinigunner()
        {
            GameWorld.instance.GDIBarracks.StartBuildingMinigunner();
        }



    }
}
