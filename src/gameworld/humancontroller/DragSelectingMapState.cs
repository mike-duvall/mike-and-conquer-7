
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using mike_and_conquer.gameview;


namespace mike_and_conquer.gameworld.humancontroller
{
    public class DragSelectingMapState : HumanControllerState
    {
        public DragSelectingMapState(Point leftMouseDownStartPoint)
        {
            UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
            unitSelectionBox.selectionBoxDragStartPoint = leftMouseDownStartPoint;
            unitSelectionBox.isDragSelectHappening = true;
        }


        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {

            if (MouseInputUtil.LeftMouseButtonIsBeingHeldDown(newMouseState, oldMouseState))
            {
                Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
                unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
                return this;
            }
            else 
            {
                UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
                unitSelectionBox.HandleEndDragSelect();
                if (!GameWorld.instance.IsAMinigunnerSelected())
                {
                    Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                    HumanPlayerController.CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldLocationPoint);
                }

                return new PointerOverMapState();
            }
        }

    }


}
