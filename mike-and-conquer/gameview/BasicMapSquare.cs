

using System.Linq;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Collections.Generic;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquare
    {
        Vector2 positionInWorldCoordinates;
        private int imageIndex;

        public Vector2 PositionInWorldCoordinates
        {
            get { return positionInWorldCoordinates; }
            set { positionInWorldCoordinates = value; }
        }

        public int ImageIndex
        {
            get
            {
                return imageIndex;
            }
            set
            {
                imageIndex = value;
            }
        }


        private string textureKey;

        public string TextureKey
        {
            get
            {
                return textureKey;
            }
            set
            {
                textureKey = value;
            }
        }


        private Minigunner minigunnerSlot0 = null;
        private Minigunner minigunnerSlot1 = null;
        private Minigunner minigunnerSlot2 = null;
        private Minigunner minigunnerSlot3 = null;
        private Minigunner minigunnerSlot4 = null;

        public BasicMapSquare(int x, int y, string textureKey, int imageIndex )
        {
            this.positionInWorldCoordinates = new Vector2(x,y);
            this.imageIndex = imageIndex;
            this.textureKey = textureKey;
        }

        public bool ContainsPoint(Point aPoint)
        {
            int height = 24;
            int width = 24;
            int leftX = (int) positionInWorldCoordinates.X - (width / 2);
            int topY = (int) positionInWorldCoordinates.Y - (height / 2);
            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(aPoint);
        }

        public Point GetCenter()
        {
            return new Point((int) positionInWorldCoordinates.X, (int) positionInWorldCoordinates.Y);
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


        public bool IsBlockingTerrain()
        {

            // TODO: Make BasicMapSquare initialized with flag of whether it's blocking or not
            Dictionary<string, int[]> blockingTerrainMap = GameWorld.instance.gameMap.blockingTerrainMap;

            if (blockingTerrainMap.ContainsKey(textureKey))
            {
                int[] blockedImageIndexes = blockingTerrainMap[textureKey];
                if (blockedImageIndexes == null)
                {
                    return true;
                }
                else
                {
                    return blockedImageIndexes.Contains(imageIndex);
                }
            }
        
            return false;
        }

        internal int GetMapSquareX()
        {
            int mapSquareX = (int)((this.positionInWorldCoordinates.X - 12) / 24);
            return mapSquareX;
        }

        internal int GetMapSquareY()
        {
            int mapSquareY = (int)((this.positionInWorldCoordinates.Y - 12) / 24);
            return mapSquareY;

        }

    }
}
