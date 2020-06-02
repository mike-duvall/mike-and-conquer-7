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
            Point mouseWorldLocationPoint = GetWorldLocationPointFromMouseState(newMouseState);

            UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
            unitSelectionBox.selectionBoxDragStartPoint = mouseWorldLocationPoint;
        }
        public override HumanControllerState Update(GameTime gameTime, MouseState newMouseState, MouseState oldMouseState)
        {

            MikeAndConquerGame.instance.log.Information("DragSelectingMapState.Update() begin");


            if (LeftMouseButtonIsBeingHeldDown(newMouseState, oldMouseState))
            {
                Point mouseWorldLocationPoint = GetWorldLocationPointFromMouseState(newMouseState);
                UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
//                unitSelectionBox.selectionBoxDragStartPoint = mouseWorldLocationPoint;
                unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
                return this;
            }

            if (LeftMouseButtonUnclicked(newMouseState, oldMouseState))
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

        private bool LeftMouseButtonUnclicked(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
        }


        private bool LeftMouseButtonIsBeingHeldDown(MouseState newMouseState, MouseState oldMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Released;
        }

        private Point GetWorldLocationPointFromMouseState(MouseState mouseState)
        {
            Vector2 mouseScreenLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mouseWorldLocationVector2 = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);
            return new Point((int)mouseWorldLocationVector2.X, (int)mouseWorldLocationVector2.Y);
        }



    }


}
