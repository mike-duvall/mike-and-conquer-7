using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameview;
using mike_and_conquer.main;
using mike_and_conquer.util;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;



namespace mike_and_conquer.gameworld.humancontroller
{
    class HumanPlayerController : PlayerController
    {

        // top level states
        private const string HANDLING_SIDEBAR_STATE = "HANDLING_SIDEBAR_STATE";
        private const string HANDLING_MAP_STATE = "HANDLING_MAP_STATE";


        // sub map states
        private const string NEUTRAL_MAP_STATE = "NEUTRAL_MAP_STATE";
        private const string UNITS_SELECTED_MAP_STATE = "UNITS_SELECTED_MAP_STATE";
        private const string DRAG_SELECTING_MAP_STATE = "DRAG_SELECTING_MAP_STATE";

        private HumanControllerState humanControllerState;

        private string topLevelState;
        private string mapState;

        public static HumanPlayerController instance;

        private MouseState oldMouseState;


        public MouseState MouseState
        {
            get { return oldMouseState; }
        }

        public HumanPlayerController()
        {
            instance = this;
            topLevelState = HANDLING_MAP_STATE;
            mapState = NEUTRAL_MAP_STATE;
            humanControllerState = new NeutralMapstate();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            humanControllerState = humanControllerState.Update(gameTime, newMouseState, oldMouseState);
//            if (topLevelState.Equals(HANDLING_MAP_STATE))
//            {
//                HandleHandlingMapState(gameTime);
//            }
//            else if (topLevelState.Equals(HANDLING_SIDEBAR_STATE))
//            {
//                HandleHandlingSidebarState(gameTime);
//            }
//            else
//            {
//                throw new System.Exception("Did not find expected topLevelState:" + topLevelState);
//            }


//            UpdateMousePointer(newMouseState);
//            Point mouseWorldLocationPoint = GetWorldLocationPointFromMouseState(newMouseState);
//
//            UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
//
//            if (LeftMouseButtonUnclicked(newMouseState))
//            {
//                if (unitSelectionBox.isDragSelectHappening)
//                {
//                    unitSelectionBox.HandleEndDragSelect();
//                }
//                else
//                {
//                    HandleLeftClick(newMouseState.Position.X, newMouseState.Position.Y);
//                }
//            }
//            else if (LeftMouseButtonClicked(newMouseState))
//            {
//                unitSelectionBox.selectionBoxDragStartPoint = mouseWorldLocationPoint;
//            }
//            else if (RightMouseButtonClicked(newMouseState))
//            {
//                unitSelectionBox.isDragSelectHappening = false;
//                HandleRightClick(newMouseState.Position.X, newMouseState.Position.Y);
//            }
//
//
//            if (LeftMouseButtonIsBeingHeldDown(newMouseState))
//            {
//                unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
//            }

            oldMouseState = newMouseState;

        }

        private void HandleHandlingSidebarState(GameTime gameTime)
        {

        }

        private void HandleHandlingMapState(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = mouseState.Position;
            Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);
            Vector2 mouseWorldLocation = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);


            int mouseWorldX = (int)mouseWorldLocation.X;
            int mouseWorldY = (int)mouseWorldLocation.Y;


            if (mapState.Equals(NEUTRAL_MAP_STATE))
            {
                GameWorldView.instance.gameCursor.SetToMainCursor();
                if (LeftMouseButtonClicked(mouseState))
                {
                    Boolean handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldX, mouseWorldY);
                    if (handledEvent)
                    {
                        mapState = UNITS_SELECTED_MAP_STATE;
                    }
                }
                if (LeftMouseButtonIsBeingHeldDown(mouseState))
                {
                    Point mouseWorldLocationPoint = GetWorldLocationPointFromMouseState(mouseState);
                    UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
                    unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
                    mapState = DRAG_SELECTING_MAP_STATE;
                }




            }
            else if (mapState.Equals(UNITS_SELECTED_MAP_STATE))
            {

                Vector2 mousePointerLocation = new Vector2(mouseState.X, mouseState.Y);
                Vector2 mousePositionAsPointInWorldCoordinatesAsVector2 =
                    GameWorldView.instance.ConvertScreenLocationToWorldLocation(mousePointerLocation);

                Point mousePositionAsPointInWorldCoordinates =
                    PointUtil.ConvertVector2ToPoint(mousePositionAsPointInWorldCoordinatesAsVector2);


                if (GameWorld.instance.IsAMinigunnerSelected())
                {
                    UpdateMousePointerWhenMinigunnerSelected(mousePositionAsPointInWorldCoordinates);
                }
                else if (GameWorld.instance.IsAnMCVSelected())
                {
                    UpdateMousePointerWhenMCVSelected(mousePositionAsPointInWorldCoordinates);
                }


                if (LeftMouseButtonClicked(mouseState))
                {
                    Boolean handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldX, mouseWorldY);
                    if (handledEvent)
                    {
                        mapState = UNITS_SELECTED_MAP_STATE;
                    }
                    if (!handledEvent)
                    {
                        handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseWorldX, mouseWorldY);
                    }

                    if (!handledEvent)
                    {
                        CheckForAndHandleLeftClickOnMap(mouseWorldX, mouseWorldY);
                    }
                }

                if (RightMouseButtonClicked(mouseState))
                {
                    HandleRightClick(mouseWorldX, mouseWorldY);
                    mapState = NEUTRAL_MAP_STATE;
                }
            }
            else if (mapState.Equals(DRAG_SELECTING_MAP_STATE))
            {
                UnitSelectionBox unitSelectionBox = GameWorld.instance.unitSelectionBox;
                Point mouseWorldLocationPoint = GetWorldLocationPointFromMouseState(mouseState);
                if (LeftMouseButtonUnclicked(mouseState))
                {

                    int numSelectedUnits = unitSelectionBox.HandleEndDragSelect();
                    if (numSelectedUnits > 0)
                    {
                        mapState = UNITS_SELECTED_MAP_STATE;
                    }
                    else
                    {
                        mapState = NEUTRAL_MAP_STATE;
                    }
                }
                else if (LeftMouseButtonClicked(mouseState))
                {
                    unitSelectionBox.selectionBoxDragStartPoint = mouseWorldLocationPoint;
                }
                else if (RightMouseButtonClicked(mouseState))
                {
                    unitSelectionBox.isDragSelectHappening = false;
                    HandleRightClick(mouseState.Position.X, mouseState.Position.Y);
                }
                

                if (LeftMouseButtonIsBeingHeldDown(mouseState))
                {
                    unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
                }


            }



          



        }

        private static void UpdateMousePointerWhenMCVSelected(Point mousePositionAsPointInWorldCoordinates)
        {
            if (GameWorld.instance.IsPointOverMCV(mousePositionAsPointInWorldCoordinates))
            {
                GameWorldView.instance.gameCursor.SetToBuildConstructionYardCursor();
            }
            else if (GameWorld.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
            {
                GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
            }
            else
            {
                GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
            }
        }

        private static void UpdateMousePointerWhenMinigunnerSelected(Point mousePositionAsPointInWorldCoordinates)
        {
            if (GameWorld.instance.IsPointOverEnemy(mousePositionAsPointInWorldCoordinates))
            {
                GameWorldView.instance.gameCursor.SetToAttackEnemyLocationCursor();
            }
            else if (GameWorld.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
            {
                GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
            }
            else
            {
                GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
            }
        }

        public override void Add(Minigunner minigunner, bool aiIsOn)
        {
            // Do nothing
            // TODO: This was added to AI controller could know about new minigunners
            // Reconsider how this is handled
        }

        private void UpdateMousePointer(MouseState mouseState)
        {
            Vector2 mousePointerLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mousePositionAsPointInWorldCoordinatesAsVector2 =
                GameWorldView.instance.ConvertScreenLocationToWorldLocation(mousePointerLocation);

            Point mousePositionAsPointInWorldCoordinates =
                PointUtil.ConvertVector2ToPoint(mousePositionAsPointInWorldCoordinatesAsVector2);

            if (GameWorld.instance.IsPointOnMap(mousePositionAsPointInWorldCoordinates) && GameWorld.instance.IsAMinigunnerSelected())
            {
                if (GameWorld.instance.IsPointOverEnemy(mousePositionAsPointInWorldCoordinates))
                {
                    GameWorldView.instance.gameCursor.SetToAttackEnemyLocationCursor();
                }
                else if (GameWorld.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
                {
                    GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
                }
                else
                {
                    GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
                }
            }
            else if (GameWorld.instance.IsPointOnMap(mousePositionAsPointInWorldCoordinates) && GameWorld.instance.IsAnMCVSelected())
            {
                if (GameWorld.instance.IsPointOverMCV(mousePositionAsPointInWorldCoordinates))
                {
                    GameWorldView.instance.gameCursor.SetToBuildConstructionYardCursor();
                }
                else if (GameWorld.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates))
                {
                    GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
                }
                else
                {
                    GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
                }
            }
        
            else
            {
                GameWorldView.instance.gameCursor.SetToMainCursor();
            }
        }

        private Point GetWorldLocationPointFromMouseState(MouseState mouseState)
        {
            Vector2 mouseScreenLocation = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mouseWorldLocationVector2 = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);
            return new Point((int)mouseWorldLocationVector2.X, (int)mouseWorldLocationVector2.Y);
        }

        private bool LeftMouseButtonIsBeingHeldDown(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Released;
        }

        private bool RightMouseButtonClicked(MouseState newMouseState)
        {
            return newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        }

        private bool LeftMouseButtonClicked(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        }

        private bool LeftMouseButtonUnclicked(MouseState newMouseState)
        {
            return newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
        }

        internal void HandleLeftClick(int mouseX, int mouseY)
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = mouseState.Position;
            Vector2 mouseScreenLocation = new Vector2(mousePoint.X, mousePoint.Y);
            Vector2 mouseWorldLocation = GameWorldView.instance.ConvertScreenLocationToWorldLocation(mouseScreenLocation);


            int mouseWorldX = (int)mouseWorldLocation.X;
            int mouseWorldY = (int)mouseWorldLocation.Y;

            bool handledEvent = CheckForAndHandleLeftClickOnSidebar(mouseScreenLocation);
            if (!handledEvent)
            {
                handledEvent = CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldX, mouseWorldY);
            }
            if (!handledEvent)
            {
                handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseWorldX, mouseWorldY);
            }
            if (!handledEvent)
            {
                CheckForAndHandleLeftClickOnMap(mouseWorldX, mouseWorldY);
            }

        }

        internal void HandleRightClick(int mouseX, int mouseY)
        {
            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                nextMinigunner.selected = false;
            }

            if (GameWorld.instance.MCV != null)
            {
                GameWorld.instance.MCV.selected = false;
            }

        }

        private bool CheckForAndHandleLeftClickOnMap(int mouseX, int mouseY)
        {
            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                if (nextMinigunner.selected == true)
                {
                    if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
                    {
                        MapTileInstance clickedMapTileInstance =
                            GameWorld.instance.FindMapTileInstance(mouseX, mouseY);
                        Point centerOfSquare = clickedMapTileInstance.GetCenter();
                        nextMinigunner.OrderToMoveToDestination(centerOfSquare);
                    }
                }
            }

            MCV mcv = GameWorld.instance.MCV;
            if (mcv != null)
            {
                if (mcv.selected == true)
                {
                    if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
                    {
                        MapTileInstance clickedMapTileInstance =
                            GameWorld.instance.FindMapTileInstance(mouseX, mouseY);
                        Point centerOfSquare = clickedMapTileInstance.GetCenter();
                        mcv.OrderToMoveToDestination(centerOfSquare);
                    }

                }
            }
            return true;
        }


        // TODO:  Add Sidebar class, have build buttons sit inside of it, iterate through
        // them and ask if they contain point where sidebar was clicked
        internal bool CheckForAndHandleLeftClickOnSidebar(Vector2 mouseScreenLocation)
        {
            bool handled = false;

            Vector2 sidebarLocation = GameWorldView.instance.ConvertScreenLocationToSidebarLocation(mouseScreenLocation);

            if (sidebarLocation.X > 0 && sidebarLocation.X < 64 && sidebarLocation.Y > 0 && sidebarLocation.Y < 48)
            {
                HandleClickOnBuildBarracks();
                handled = true;
            }
            else if (sidebarLocation.X > 80 && sidebarLocation.X < 144 && sidebarLocation.Y > 0 && sidebarLocation.Y < 48)
            {
                HandleClickOnBuildMinigunner();
                handled = true;
            }

            return handled;

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


        internal Boolean CheckForAndHandleLeftClickOnFriendlyUnit(int mouseX, int mouseY)
        {
            Boolean handled = false;
            foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            {
                if (nextMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    GameWorld.instance.SelectSingleGDIUnit(nextMinigunner);
                }
            }

            if (!handled)
            {
                handled = CheckForAndHandleLeftClickOnMCV(mouseX, mouseY);
            }

            return handled;
        }

        private static bool CheckForAndHandleLeftClickOnMCV(int mouseX, int mouseY)
        {
            Boolean handled = false;
            MCV mcv = GameWorld.instance.MCV;
            if (mcv != null)
            {
                if (mcv.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    if (mcv.selected == false)
                    {
                        GameWorld.instance.SelectMCV(GameWorld.instance.MCV);
                    }
                    else
                    {
                        Point mcvPositionInWorldCoordinates = new Point((int)mcv.positionInWorldCoordinates.X,
                            (int)mcv.positionInWorldCoordinates.Y);
                        MikeAndConquerGame.instance.RemoveMCV();
                        MikeAndConquerGame.instance.AddGDIConstructionYardAtWorldCoordinates(mcvPositionInWorldCoordinates);
                    }
                }
            }

            return handled;
        }


        internal Boolean CheckForAndHandleLeftClickOnEnemyUnit(int mouseX, int mouseY)
        {
            bool handled = false;
            foreach (Minigunner nextNodMinigunner in GameWorld.instance.NodMinigunnerList)
            {
                if (nextNodMinigunner.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;
                    foreach (Minigunner nextGdiMinigunner in GameWorld.instance.GDIMinigunnerList)
                    {
                        if (nextGdiMinigunner.selected)
                        {
                            nextGdiMinigunner.OrderToMoveToAndAttackEnemyUnit(nextNodMinigunner);
                        }
                    }
                }
            }

            return handled;
        }


    }
}
