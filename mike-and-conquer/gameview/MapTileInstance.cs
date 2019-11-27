﻿
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;


namespace mike_and_conquer.gameview
{
    public class MapTileInstance
    {
        public Vector2 PositionInWorldCoordinates { get; }

        private string textureKey;
        private byte imageIndex;
        private bool isBlockingTerrain;
        
        
        public string TextureKey
        {
            get { return textureKey; }
        }
        
        public byte ImageIndex
        {
            get { return imageIndex; }
        }
        
        public bool IsBlockingTerrain
        {
            get { return isBlockingTerrain; }
            set { isBlockingTerrain = value; }
        }


        public enum MapTileVisibility
        {
            NotVisible,
            PartiallyVisible,
            Visible
        }

        private MapTileVisibility mapTileVisibility;

        public MapTileVisibility Visibility
        {
            get { return mapTileVisibility; }
            set { mapTileVisibility = value; }
        }

        private Minigunner minigunnerSlot0 = null;
        private Minigunner minigunnerSlot1 = null;
        private Minigunner minigunnerSlot2 = null;
        private Minigunner minigunnerSlot3 = null;
        private Minigunner minigunnerSlot4 = null;

        public MapTileInstance(int x, int y, string textureKey, byte imageIndex, bool isBlockingTerrain)
        {
            this.PositionInWorldCoordinates = new Vector2(x,y);
            this.textureKey = textureKey;
            this.imageIndex = imageIndex;
            this.isBlockingTerrain = isBlockingTerrain;
            this.Visibility = MapTileInstance.MapTileVisibility.NotVisible;

        }

        public bool ContainsPoint(Point aPoint)
        {
            int width = GameWorld.MAP_TILE_WIDTH;
            int height = GameWorld.MAP_TILE_HEIGHT;

            int leftX = (int)PositionInWorldCoordinates.X - (width / 2);
            int topY = (int)PositionInWorldCoordinates.Y - (height / 2);
            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }

        public Point GetCenter()
        {
            return new Point((int)PositionInWorldCoordinates.X, (int)PositionInWorldCoordinates.Y);
        }

        public Point GetDestinationSlotForMinigunner(Minigunner aMinigunner)
        {

            Point nextAvailablePosition = GetCenter();
            if (minigunnerSlot0 == null)
            {
                // TODO:  These slot offsets where determined by trial and error.  
                // May want to revisit and see if there is some formula
                // that can be used to calculate them instead of hard coding them...
                nextAvailablePosition.X = nextAvailablePosition.X + 4;
                nextAvailablePosition.Y = nextAvailablePosition.Y - 3;
                minigunnerSlot0 = aMinigunner;
            }
            else if (minigunnerSlot1 == null) 
            {
                nextAvailablePosition.X = nextAvailablePosition.X - 8;
                nextAvailablePosition.Y = nextAvailablePosition.Y - 3;
                minigunnerSlot1 = aMinigunner;
            }
            else if (minigunnerSlot2 == null)
            {
                nextAvailablePosition.X = nextAvailablePosition.X + 4;
                nextAvailablePosition.Y = nextAvailablePosition.Y + 10;
                minigunnerSlot2 = aMinigunner;
            }
            else if (minigunnerSlot3 == null)
            {
                nextAvailablePosition.X = nextAvailablePosition.X - 8;
                nextAvailablePosition.Y = nextAvailablePosition.Y + 10;
                minigunnerSlot3 = aMinigunner;
            }
            else if (minigunnerSlot4 == null)
            {
                nextAvailablePosition.X = nextAvailablePosition.X - 2;
                nextAvailablePosition.Y = nextAvailablePosition.Y + 3;
                minigunnerSlot4 = aMinigunner;

            }

            return nextAvailablePosition;
        }

        internal void ClearSlotForMinigunner(Minigunner aMinigunner)
        {
            if (minigunnerSlot0 == aMinigunner)
            {
                minigunnerSlot0 = null;
            }
            if (minigunnerSlot1 == aMinigunner)
            {
                minigunnerSlot1 = null;
            }
            if (minigunnerSlot2 == aMinigunner)
            {
                minigunnerSlot2 = null;
            }
            if (minigunnerSlot3 == aMinigunner)
            {
                minigunnerSlot3 = null;
            }
            if (minigunnerSlot4 == aMinigunner)
            {
                minigunnerSlot4 = null;
            }

        }


        public void ClearAllMinigunnerSlots()
        {
            minigunnerSlot0 = null;
            minigunnerSlot1 = null;
            minigunnerSlot2 = null;
            minigunnerSlot3 = null;
            minigunnerSlot4 = null;
        }
    }
}
