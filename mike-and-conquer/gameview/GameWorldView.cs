
using System;
using System.Collections.Generic;
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

        private GDIConstructionYardView gdiConstructionYardView;
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

        public GDIBarracksView GDIBarracksView
        {
            get { return gdiBarracksView; }
        }

        public GDIConstructionYardView GdiConstructionYardView
        {
            get { return gdiConstructionYardView; }
        }



        public List<TerrainView> terrainViewList;

        public MCVView mcvView;

        public static GameWorldView instance;




        public GameWorldView()
        {
            mapTileInstanceViewList = new List<MapTileInstanceView>();

            gdiMinigunnerViewList = new List<MinigunnerView>();
            nodMinigunnerViewList = new List<MinigunnerView>();

            sandbagViewList = new List<SandbagView>();
            terrainViewList = new List<TerrainView>();
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
            mcvView = null;
            gdiConstructionYardView = null;

        }


        public void AddMapTileInstanceView(MapTileInstance mapTileInstance)
        {
            MapTileInstanceView mapTileInstanceView = new MapTileInstanceView(mapTileInstance);
            this.mapTileInstanceViewList.Add(mapTileInstanceView);
        }

        public void AddTerrainItemView(TerrainItem terrainItem)
        {
            TerrainView terrainView = new TerrainView(terrainItem);
            this.terrainViewList.Add(terrainView);
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

        public void AddGDIBarracksView(GDIBarracks gdiBarracks)
        {
            gdiBarracksView = new GDIBarracksView(gdiBarracks);
        }

        public void AddGDIConstructionYardView(GDIConstructionYard gdiConstructionYard)
        {
            gdiConstructionYardView = new GDIConstructionYardView(gdiConstructionYard);
        }



        public void AddMCVView(MCV mcv)
        {
            mcvView = new MCVView(mcv);
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
