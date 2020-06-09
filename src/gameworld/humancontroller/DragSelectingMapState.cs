using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameview;
using mike_and_conquer.main;

namespace mike_and_conquer.gameworld.humancontroller
{
    public class DragSelectingMapState : HumanControllerState
    {
        private DragSelectingMapState()
        {
        }

        public DragSelectingMapState(MouseState newMouseState)
        {
            Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);

            UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
            unitSelectionBox.selectionBoxDragStartPoint = mouseWorldLocationPoint;
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

            if (MouseInputUtil.LeftMouseButtonUnclicked(newMouseState, oldMouseState))
            {
                UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
                unitSelectionBox.HandleEndDragSelect();
                if (GameWorld.instance.IsAnyUnitSelected())
                {
                    return new UnitsSelectedMapState();
                }
                else
                {
                    return new NeutralMapstate();
                }

            }

            return this;

        }

    }


}
