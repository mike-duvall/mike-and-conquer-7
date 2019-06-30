
using System;
using System.Collections.Generic;
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;


namespace mike_and_conquer.gameview
{
    public class GameWorldView
    {


        private List<MinigunnerView> gdiMinigunnerViewList;
        private List<MinigunnerView> nodMinigunnerViewList;

        private GDIBarracksView gdiBarracksView;
//        private MinigunnerIconView minigunnerIconView;

        private List<MapTileInstanceView> mapTileInstanceViewList;

        private List<SandbagView> sandbagViewList;


        public List<MapTileInstanceView> MapTileInstanceViewList
        {
            get { return mapTileInstanceViewList; }
        }


        public List<MinigunnerView> GdiMinigunnerViewList
        {
            get { return gdiMinigunnerViewList; }
        }


        public List<MinigunnerView> NodMinigunnerViewList
        {
            get { return nodMinigunnerViewList; }
        }

        public List<SandbagView> SandbagViewList
        {
            get { return sandbagViewList; }
        }

        public static GameWorldView instance;



        public GameWorldView()
        {
            mapTileInstanceViewList = new List<MapTileInstanceView>();

            gdiMinigunnerViewList = new List<MinigunnerView>();
            nodMinigunnerViewList = new List<MinigunnerView>();

            sandbagViewList = new List<SandbagView>();
            instance = this;

        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }


        public void HandleReset()
        {
            gdiMinigunnerViewList.Clear();
            nodMinigunnerViewList.Clear();
            sandbagViewList.Clear();

        }

        public void AddGDIBarracksViewAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            int xInWorldCoordinates = positionInMapSquareCoordinates.X * GameWorld.MAP_TILE_WIDTH;
            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * GameWorld.MAP_TILE_HEIGHT;

            Point positionInWorldCoordinates = new Point(xInWorldCoordinates, yInWorldCoordinates);

            gdiBarracksView = new GDIBarracksView(positionInWorldCoordinates);

        }


//        public void AddMMinigunnerIconView()
//        {
//            minigunnerIconView = new MinigunnerIconView();
//        }

        public void AddMapTileInstanceView(MapTileInstance mapTileInstance)
        {
            MapTileInstanceView mapTileInstanceView = new MapTileInstanceView(mapTileInstance);
            this.mapTileInstanceViewList.Add(mapTileInstanceView);

        }

        public void AddGDIMinigunnerView(Minigunner newMinigunner)
        {
            MinigunnerView newMinigunnerView = new GdiMinigunnerView(newMinigunner);
            gdiMinigunnerViewList.Add(newMinigunnerView);

        }

        public void AddSandbagView(Sandbag newSandbag)
        {
            SandbagView newSandbagView = new SandbagView(newSandbag);
            sandbagViewList.Add(newSandbagView);

        }

        public void AddNodMinigunnerView(Minigunner newMinigunner)
        {
            MinigunnerView newMinigunnerView = new NodMinigunnerView(newMinigunner);
            nodMinigunnerViewList.Add(newMinigunnerView);

        }

        public MapTileInstanceView FindMapSquareView(int xWorldCoordinate, int yWorldCoordinate)
        {

            foreach (MapTileInstanceView nextBasicMapSquareView in this.mapTileInstanceViewList)
            {
                MapTileInstance mapTileInstance = nextBasicMapSquareView.myMapTileInstance;
                if (mapTileInstance.ContainsPoint(new Point(xWorldCoordinate, yWorldCoordinate)))
                {
                    return nextBasicMapSquareView;
                }
            }
            throw new Exception("Unable to find MapTileInstance at coordinates, x:" + xWorldCoordinate + ", y:" + yWorldCoordinate);

        }


    }
}
