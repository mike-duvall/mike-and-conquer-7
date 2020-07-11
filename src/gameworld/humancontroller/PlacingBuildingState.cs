using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.main;

namespace mike_and_conquer.gameworld.humancontroller
{
    class PlacingBuldingState : HumanControllerState
    {

        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {
            if (GameWorldView.instance.barracksPlacementIndicatorView == null)
            {
                GameWorldView.instance.barracksPlacementIndicatorView = new BarracksPlacementIndicatorView();
                GameWorldView.instance.barracksPlacementIndicatorView.position = new Point(
                    (int) GameWorld.instance.GDIConstructionYard.positionInWorldCoordinates.X,
                    (int) GameWorld.instance.GDIConstructionYard.positionInWorldCoordinates.Y);


            }

            if (!MouseInputUtil.IsOverSidebar(newMouseState))
            {

                Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);

                int originalX = mouseWorldLocationPoint.X;
                int originalY = mouseWorldLocationPoint.Y;

                int halfWidth = GameWorld.MAP_TILE_WIDTH / 2;
                int halfHeight = GameWorld.MAP_TILE_HEIGHT / 2;
                int snappedX = originalX - (originalX % GameWorld.MAP_TILE_WIDTH) + halfWidth;
                int snappedY = originalY - (originalY % GameWorld.MAP_TILE_HEIGHT) + halfHeight;

                GameWorldView.instance.barracksPlacementIndicatorView.position.X = snappedX;
                GameWorldView.instance.barracksPlacementIndicatorView.position.Y = snappedY;

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
