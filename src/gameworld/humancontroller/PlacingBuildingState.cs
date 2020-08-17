using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;


namespace mike_and_conquer.gameworld.humancontroller
{
    class PlacingBuildingState : HumanControllerState
    {

        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {

            GameWorldView.instance.Notify_PlacingBarracks();

            if (!MouseInputUtil.IsOverSidebar(newMouseState))
            {

                GameWorldView.instance.Notify_PlacingBarracksWithMouseOverMap(newMouseState.Position);

                if (MouseInputUtil.LeftMouseButtonUnclicked(newMouseState, oldMouseState))
                {
                    if(GameWorldView.instance.barracksBarracksPlacementIndicator.ValidBuildingLocation())
                    {
                        GDIConstructionYard gdiConstructionYard = GameWorld.instance.GDIConstructionYard;
                        Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                        MapTileLocation mapTileLocation = MapTileLocation.CreateFromWorldCoordinates(mouseWorldLocationPoint.X, mouseWorldLocationPoint.Y);
                        gdiConstructionYard.CreateBarracksAtPosition(mapTileLocation);
                        GameWorldView.instance.Notify_DonePlacingBarracks();
                        return new PointerOverMapState();
                    }

                }
            }
            return this;

        }

    }
}
