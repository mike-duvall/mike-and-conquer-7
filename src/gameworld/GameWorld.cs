using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameevent;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld.humancontroller;
using mike_and_conquer.main;
using mike_and_conquer.pathfinding;
using mike_and_conquer.util;
using AsyncGameEvent = mike_and_conquer.gameevent.AsyncGameEvent;
using CreateGDIMinigunnerGameEvent = mike_and_conquer.gameevent.CreateGDIMinigunnerGameEvent;
using GetGDIMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetGDIMinigunnerByIdGameEvent;
using CreateNodMinigunnerGameEvent = mike_and_conquer.gameevent.CreateNodMinigunnerGameEvent;
using GetNodMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetNodMinigunnerByIdGameEvent;
using ResetGameGameEvent = mike_and_conquer.gameevent.ResetGameGameEvent;
using GetCurrentGameStateGameEvent = mike_and_conquer.gameevent.GetCurrentGameStateGameEvent;
using CreateSandbagGameEvent = mike_and_conquer.gameevent.CreateSandbagGameEvent;


using Exception = System.Exception;

using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;

namespace mike_and_conquer.gameworld

{
    public class GameWorld
    {

        public static int MAP_TILE_WIDTH = 24;
        public static int MAP_TILE_HEIGHT = 24;

        private GDIPlayer gdiPlayer;
        private NodPlayer nodPlayer;

        public List<Sandbag> sandbagList;
        public List<TerrainItem> terrainItemList;

        public List<Minigunner> GDIMinigunnerList
        {
            get { return gdiPlayer.GdiMinigunnerList; }
        }

        public List<Minigunner> NodMinigunnerList
        {
            get { return nodPlayer.NodMinigunnerList; }
        }

        public GDIBarracks GDIBarracks
        {
            get { return gdiPlayer.GDIBarracks; }
        }

        public GDIConstructionYard GDIConstructionYard
        {
            get { return gdiPlayer.GDIConstructionYard; }
        }

        public MCV MCV
        {
            get { return gdiPlayer.MCV; }
        }

        public NavigationGraph navigationGraph;

        private List<AsyncGameEvent> gameEvents;

        public GameMap gameMap;

        public UnitSelectionBox unitSelectionBox;

        public static GameWorld instance;

        public GameWorld()
        {
            // gdiMinigunnerList = new List<Minigunner>();
            gdiPlayer = new GDIPlayer(new HumanPlayerController());
            nodPlayer = new NodPlayer(new NodAIPlayerController());
            // nodMinigunnerList = new List<Minigunner>();
            sandbagList = new List<Sandbag>();
            terrainItemList = new List<TerrainItem>();

            gameEvents = new List<AsyncGameEvent>();

            // nodMinigunnerAIControllerList = new List<MinigunnerAIController>();
            unitSelectionBox = new UnitSelectionBox();

            GameWorld.instance = this;
        }

        public GameState HandleReset()
        {
            // gdiMinigunnerList.Clear();
            gdiPlayer.HandleReset();
            nodPlayer.HandleReset();
            // nodMinigunnerList.Clear();
            sandbagList.Clear();
            // mcv = null;
            // gdiConstructionYard = null;
            // gdiBarracks = null;
            gameMap.Reset();
            InitializeNavigationGraph();
            return new PlayingGameState();
        }


        public void InitializeDefaultMap()
        {
            LoadMap();
            navigationGraph = new NavigationGraph(this.gameMap.numColumns, this.gameMap.numRows);
        }

        public void InitializeTestMap(int[,] obstacleArray)
        {
            gameMap = new GameMap(obstacleArray);
            navigationGraph = new NavigationGraph(this.gameMap.numColumns, this.gameMap.numRows);
            InitializeNavigationGraph();
        }


        private void LoadMap()
        {

            Stream inputStream = new FileStream(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "scg01ea.bin", FileMode.Open);


            //  (Starting at 0x13CC in the file)

            // TODO Should eventually read this out of the INI file instead of hard coding
            int startX = 35;
            int startY = 39;
            int endX = 61;
            int endY = 61;

            gameMap = new GameMap(inputStream, startX, startY, endX, endY);

            // TODO:  Eventually create a data file
            // which has the tile descriptor data(specifically, the blocked tiles)
            // and read all of this from a file, rather than being hard coded
            // in the code
            Dictionary<String, TerrainItemDescriptor> terrainItemDescriptorDictionary = new Dictionary<string, TerrainItemDescriptor>();
            addT01(terrainItemDescriptorDictionary);  
            addT02(terrainItemDescriptorDictionary);  
//            addT05(terrainItemDescriptorDictionary);  // not visible
            addT06(terrainItemDescriptorDictionary);  
            addT07(terrainItemDescriptorDictionary);  
            addT16(terrainItemDescriptorDictionary);  
            addTC01(terrainItemDescriptorDictionary); 
            addTC02(terrainItemDescriptorDictionary); 
            addTC04(terrainItemDescriptorDictionary); 
            addTC05(terrainItemDescriptorDictionary);

            SortedDictionary<int, string> terrainMap = new SortedDictionary<int, string>();

            terrainMap.Add(3303, "T01.tem");
            terrainMap.Add(2988, "T01.tem");
            terrainMap.Add(2991, "T01.tem");
            terrainMap.Add(3121, "T01.tem");
            terrainMap.Add(2936, "T01.tem");
            terrainMap.Add(2861, "T02.tem");
            terrainMap.Add(3111, "T02.tem");
            terrainMap.Add(3369, "T06.tem");
            terrainMap.Add(3496, "T06.tem");
            terrainMap.Add(2860, "T07.tem");
            terrainMap.Add(3245, "T16.tem");
            terrainMap.Add(2937, "T16.tem");
            terrainMap.Add(2555, "TC01.tem");
            terrainMap.Add(3246, "TC01.tem");
            terrainMap.Add(2666, "TC01.tem");
            terrainMap.Add(3052, "TC02.tem");
            terrainMap.Add(3056, "TC02.tem");
            terrainMap.Add(2871, "TC02.tem");
            terrainMap.Add(2544, "TC02.tem");
            terrainMap.Add(2794, "TC04.tem");
            terrainMap.Add(2938, "TC04.tem");
            terrainMap.Add(2605, "TC05.tem");  
            terrainMap.Add(2680, "TC05.tem");


            float layerDepthOffset = 0.0f;
            foreach (int cellnumber in terrainMap.Keys)
            {
                Point point = ConvertCellNumberToTopLeftWorldCoordinates(cellnumber);
                if (point.X >= 0 && point.Y >= 0)
                {
                    String terrainItemType = terrainMap[cellnumber];
                    TerrainItemDescriptor terrainItemDescriptor = terrainItemDescriptorDictionary[terrainItemType];
                    TerrainItem terrainItem = new TerrainItem(point.X, point.Y, terrainItemDescriptor, layerDepthOffset);
                    layerDepthOffset = layerDepthOffset + 0.01f;
                    terrainItemList.Add(terrainItem);

                    List<MapTileInstance> blockeMapTileInstances = terrainItem.GetBlockedMapTileInstances();
                    foreach(MapTileInstance blockedMapTileInstance in blockeMapTileInstances)
                    {
                        if (blockedMapTileInstance != null)
                        {
                            blockedMapTileInstance.IsBlockingTerrain = true;
                        }

                    }

                }
            }

        }

        private static void addT01(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T01.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addT02(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T02.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addT05(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T05.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addT06(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T06.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }


        private static void addT07(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T07.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addT16(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T16.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }


        private static void addTC01(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            blockMapTileOffsets.Add(new Point(1, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "TC01.tem",
                72,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addTC04(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            blockMapTileOffsets.Add(new Point(1, 1));
            blockMapTileOffsets.Add(new Point(2, 1));
            blockMapTileOffsets.Add(new Point(0, 2));

            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "TC04.tem",
                96,
                72,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }


        private static void addTC05(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            blockMapTileOffsets.Add(new Point(1, 1));
            blockMapTileOffsets.Add(new Point(2, 1));
            blockMapTileOffsets.Add(new Point(1, 2));
            blockMapTileOffsets.Add(new Point(2, 2));


            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "TC05.tem",
                96,
                72,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }


        private static void addTC02(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(1, 0));
            blockMapTileOffsets.Add(new Point(0, 1));
            blockMapTileOffsets.Add(new Point(1, 1));


            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "TC02.tem",
                72,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }



        private Point ConvertCellNumberToTopLeftWorldCoordinates(int cellnumber)
        {
            // TODO: Eventually update this formula to use non hard coded map size
            int quotient = cellnumber / 64;
            int remainder = cellnumber % 64;

            int mapTileX = remainder - 35;
            int mapTileY = quotient - 39;

            int mapX = mapTileX * GameWorld.MAP_TILE_WIDTH;
            int mapY = mapTileY * GameWorld.MAP_TILE_HEIGHT;

            return new Point(mapX, mapY);
        }

        internal Minigunner GetGdiMinigunner(int id)
        {
            // Minigunner foundMinigunner = null;
            // foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            // {
            //     if (nextMinigunner.id == id)
            //     {
            //         foundMinigunner = nextMinigunner;
            //     }
            // }
            //
            // return foundMinigunner;

            return gdiPlayer.GetMinigunner(id);
        }


        internal Minigunner GetNodMinigunner(int id)
        {
            // Minigunner foundMinigunner = null;
            // foreach (Minigunner nextMinigunner in nodMinigunnerList)
            // {
            //     if (nextMinigunner.id == id)
            //     {
            //         foundMinigunner = nextMinigunner;
            //     }
            //
            // }
            // return foundMinigunner;
            return nodPlayer.GetNodMinigunner(id);
        }


        internal Minigunner GetGdiOrNodMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foundMinigunner = GetGdiMinigunner(id);
            if (foundMinigunner == null)
            {
                foundMinigunner = GetNodMinigunner(id);
            }
            return foundMinigunner;
        }


        void DeslectAllUnits()
        {
            // foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            // {
            //     nextMinigunner.selected = false;
            // }
            //
            // if (GameWorld.instance.MCV != null)
            // {
            //     GameWorld.instance.MCV.selected = false;
            // }

            gdiPlayer.DeslectAllUnits();

        }

        internal void SelectSingleGDIUnit(Minigunner minigunner)
        {
            DeslectAllUnits();
            minigunner.selected = true;
        }

        internal void SelectMCV(MCV mcv)
        {
            DeslectAllUnits();
            mcv.selected = true;
        }



        public void Update(GameTime gameTime)
        {
            // UpdateAIControllers(gameTime);
            // UpdateGDIMinigunners(gameTime);
            // UpdateNodMinigunners(gameTime);
            // UpdateBarracks(gameTime);
            // UpdateConstructionYard(gameTime);

            gdiPlayer.Update(gameTime);
            nodPlayer.Update(gameTime);
            // if (mcv != null)
            // {
            //     mcv.Update(gameTime);
            // }

        }


        // private void UpdateBarracks(GameTime gameTime)
        // {
        //     if (gdiBarracks != null)
        //     {
        //         gdiBarracks.Update(gameTime);
        //     }
        // }
        //
        //
        // private void UpdateConstructionYard(GameTime gameTime)
        // {
        //     if (gdiConstructionYard != null)
        //     {
        //         gdiConstructionYard.Update(gameTime);
        //     }
        // }


        // private void UpdateNodMinigunners(GameTime gameTime)
        // {
        //     foreach (Minigunner nextMinigunner in nodMinigunnerList)
        //     {
        //         if (nextMinigunner.health > 0)
        //         {
        //             nextMinigunner.Update(gameTime);
        //         }
        //     }
        // }

        // private void UpdateGDIMinigunners(GameTime gameTime)
        // {
        //     foreach (Minigunner nextMinigunner in gdiMinigunnerList)
        //     {
        //         if (nextMinigunner.health > 0)
        //         {
        //             nextMinigunner.Update(gameTime);
        //         }
        //     }
        // }

        // private void UpdateAIControllers(GameTime gameTime)
        // {
        //     foreach (MinigunnerAIController nextMinigunnerAIController in nodMinigunnerAIControllerList)
        //     {
        //         nextMinigunnerAIController.Update(gameTime);
        //     }
        // }




        public class BadMinigunnerLocationException : Exception
        {
            public BadMinigunnerLocationException(Point badLocation)
                : base("Bad minigunner location.  x:" + badLocation.X + ", y:" + badLocation.Y)
            {
            }
        }


        private void AssertIsValidMinigunnerPosition(Point positionInWorldCoordinates)
        {
            foreach (MapTileInstance nexBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nexBasicMapSquare.ContainsPoint(positionInWorldCoordinates) &&
                    nexBasicMapSquare.IsBlockingTerrain)
                {
                    throw new BadMinigunnerLocationException(positionInWorldCoordinates);
                }
            }
        }


        public Minigunner AddGdiMinigunner(Point positionInWorldCoordinates)
        {
            
            AssertIsValidMinigunnerPosition(positionInWorldCoordinates);

            Minigunner newMinigunner = new Minigunner(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, this );
            gdiPlayer.AddMinigunner(newMinigunner);
            // gdiMinigunnerList.Add(newMinigunner);
            return newMinigunner;
        }


        public MCV AddMCV(Point positionInWorldCoordinates)
        {
            // mcv = new MCV(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, this);
            // return mcv;

            return gdiPlayer.AddMCV(positionInWorldCoordinates);
        }


        public Minigunner AddNodMinigunner(Point positionInWorldCoordinates, bool aiIsOn)
        {

            AssertIsValidMinigunnerPosition(positionInWorldCoordinates);


            Minigunner newMinigunner = new Minigunner(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, this);
            // this.nodMinigunnerList.Add(newMinigunner);
            
            // // TODO:  In future, don't couple Nod having to be AI controlled enemy
            // if (aiIsOn)
            // {
            //     MinigunnerAIController minigunnerAIController = new MinigunnerAIController(newMinigunner);
            //     nodMinigunnerAIControllerList.Add(minigunnerAIController);
            // }
            
            // return newMinigunner;
            return nodPlayer.AddNodMinigunner(newMinigunner, aiIsOn);
        }

        public GDIBarracks AddGDIBarracks(Point positionInWorldCoordinates)
        {
            // // TODO Might want to check if one already exists and throw error if so
            // gdiBarracks = new GDIBarracks(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
            // return gdiBarracks;
            return gdiPlayer.AddGDIBarracks(positionInWorldCoordinates);
        }

        public GDIConstructionYard AddGDIConstructionYard(Point positionInWorldCoordinates)
        {
            // // TODO Might want to check if one already exists and throw error if so
            // gdiConstructionYard = new GDIConstructionYard(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
            // return gdiConstructionYard;

            return gdiPlayer.AddGDIConstructionYard(positionInWorldCoordinates);
        }


        public GameState ProcessGameEvents()
        {
            GameState newGameState = null;

            lock (gameEvents)
            {
                foreach (AsyncGameEvent nextGameEvent in gameEvents)
                {
                    GameState returnedGameState = nextGameEvent.Process();
                    if (returnedGameState != null && newGameState == null)
                    {
                        newGameState = returnedGameState;
                    }
                }
                gameEvents.Clear();
            }

            return newGameState;

        }


        public Minigunner CreateGDIMinigunnerViaEvent(Point minigunnerPosition)
        {
            CreateGDIMinigunnerGameEvent gameEvent = new CreateGDIMinigunnerGameEvent(minigunnerPosition);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;

        }

        public MCV CreateMCVViaEvent(Point position)
        {
            CreateMCVGameEvent gameEvent = new CreateMCVGameEvent(position);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            MCV mcv = gameEvent.GetMCV();
            return mcv;

        }



        public void SetGDIMinigunnerHealthViaEvent(int minigunnerId, int newHealth)
        {
            SetGDIMinigunnerHealthGameEvent gameEvent = new SetGDIMinigunnerHealthGameEvent(minigunnerId, newHealth);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

        }


        

        public Sandbag CreateSandbagViaEvent(int x, int y, int index)
        {
            CreateSandbagGameEvent gameEvent = new CreateSandbagGameEvent(x, y, index);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Sandbag sandbag = gameEvent.GetSandbag();
            return sandbag;

        }

        public Minigunner GetGDIMinigunnerByIdViaEvent(int id)
        {
            GetGDIMinigunnerByIdGameEvent gameEvent = new GetGDIMinigunnerByIdGameEvent(id);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;
        }


        public Minigunner CreateNodMinigunnerViaEvent(Point positionInWorldCoordinates, bool aiIsOn)
        {
            CreateNodMinigunnerGameEvent gameEvent = new CreateNodMinigunnerGameEvent(positionInWorldCoordinates, aiIsOn);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner minigunner = gameEvent.GetMinigunner();
            return minigunner;

        }


        public Minigunner GetNodMinigunnerByIdViaEvent(int id)
        {
            GetNodMinigunnerByIdGameEvent gameEvent = new GetNodMinigunnerByIdGameEvent(id);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;
        }


        public void ResetGameViaEvent(bool drawShroud)
        {
            ResetGameGameEvent gameEvent = new ResetGameGameEvent(drawShroud);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

        }

        public MemoryStream GetScreenshotViaEvent()
        {
            GetScreenshotGameEvent gameEvent = new GetScreenshotGameEvent();

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            MemoryStream memoryStream = gameEvent.GetMemoryStream();
            return memoryStream;
        }


        public GameState GetCurrentGameStateViaEvent()
        {
            GetCurrentGameStateGameEvent gameEvent = new GetCurrentGameStateGameEvent();

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            return gameEvent.GetGameState();
        }

        public MapTileInstance FindMapTileInstance(int xWorldCoordinate, int yWorldCoordinate)
        {
        
            foreach (MapTileInstance nextBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nextBasicMapSquare.ContainsPoint(new Point(xWorldCoordinate, yWorldCoordinate)))
                {
                    return nextBasicMapSquare;
                }
            }
            throw new Exception("Unable to find MapTileInstance at coordinates, x:" + xWorldCoordinate + ", y:" + yWorldCoordinate);
        
        }

        public MapTileInstance FindMapTileInstanceAllowNull(int xWorldCoordinate, int yWorldCoordinate)
        {

            foreach (MapTileInstance nextBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nextBasicMapSquare.ContainsPoint(new Point(xWorldCoordinate, yWorldCoordinate)))
                {
                    return nextBasicMapSquare;
                }
            }

            return null;

        }

        public void InitializeNavigationGraph()
        {

            navigationGraph.Reset();

            foreach (Sandbag nextSandbag in sandbagList)
            {
                Point sandbagPositionInMapTileCoordinates =
                    ConvertWorldPositionVector2ToMapTilePositionPoint(nextSandbag.positionInWorldCoordinates);
                navigationGraph.MakeNodeBlockingNode(
                    sandbagPositionInMapTileCoordinates.X,
                    sandbagPositionInMapTileCoordinates.Y);
            }


            foreach (MapTileInstance nextMapTileInstance in this.gameMap.MapTileInstanceList)
            {
                if (nextMapTileInstance.IsBlockingTerrain)
                {
                    Point mapTilePositionInMapTileCoordinates =
                        ConvertWorldPositionVector2ToMapTilePositionPoint(nextMapTileInstance
                            .PositionInWorldCoordinates);
                    navigationGraph.MakeNodeBlockingNode(
                        mapTilePositionInMapTileCoordinates.X,
                        mapTilePositionInMapTileCoordinates.Y);
                }
            }

            navigationGraph.RebuildAdajencyGraph();

        }

        private Point ConvertWorldPositionVector2ToMapTilePositionPoint(Vector2 positionInWorldCoordinates)
        {
            return ConvertWorldCoordinatesToMapTileCoordinates(new Point((int)positionInWorldCoordinates.X,
                (int)positionInWorldCoordinates.Y));

        }


        public Point ConvertMapTileCoordinatesToWorldCoordinates(Point pointInWorldMapSquareCoordinates)
        {

            int xInWorldCoordinates = (pointInWorldMapSquareCoordinates.X * MAP_TILE_WIDTH) +
                                      (MAP_TILE_WIDTH / 2);
            int yInWorldCoordinates = pointInWorldMapSquareCoordinates.Y * MAP_TILE_HEIGHT +
                                      (MAP_TILE_HEIGHT / 2);

            return new Point(xInWorldCoordinates, yInWorldCoordinates);
        }


        public Point ConvertWorldCoordinatesToMapTileCoordinates(Point pointInWorldCoordinates)
        {
        
            int destinationRow = pointInWorldCoordinates.Y;
            int destinationColumn = pointInWorldCoordinates.X;
        
            int destinationX = destinationColumn / MAP_TILE_WIDTH;
            int destinationY = destinationRow / MAP_TILE_HEIGHT;
        
            return new Point(destinationX, destinationY);
        }


        public Point ConvertMapSquareIndexToWorldCoordinate(int index)
        {
            int numColumns = navigationGraph.width;
            Point point = new Point();
            int row = index / numColumns;
            int column = index - (row * numColumns);

            point.X = (column * GameWorld.MAP_TILE_WIDTH) + (GameWorld.MAP_TILE_WIDTH / 2);
            point.Y = (row * GameWorld.MAP_TILE_HEIGHT) + (GameWorld.MAP_TILE_HEIGHT / 2);
            return point;
        }


        public bool IsPointOnMap(Point pointInWorldCoordinates)
        {
            int largestXValue = (this.gameMap.numColumns * GameWorld.MAP_TILE_WIDTH) -1;
            int largestYValue = (this.gameMap.numRows * GameWorld.MAP_TILE_HEIGHT) -1;

            return (pointInWorldCoordinates.X >= 0 &&
                    pointInWorldCoordinates.X < largestXValue &&
                    pointInWorldCoordinates.Y >= 0 &&
                    pointInWorldCoordinates.Y < largestYValue
                );

        }

        public void MakeMapSquareVisible(Point positionInWorldCoordinates, MapTileInstance.MapTileVisibility visibility)
        {
            MapTileInstance mapTileInstance = this.FindMapTileInstance(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
            mapTileInstance.Visibility = visibility;
        }


        public bool IsPointAdjacentToConstructionYard(Point pointInWordlCoordinates)
        {
            MapTileInstance mapTileInstance = this.FindMapTileInstanceAllowNull(pointInWordlCoordinates.X, pointInWordlCoordinates.Y);
            if (mapTileInstance == null)
            {
                return false;
            }

            return IsMapTileInstanceAdjacentToConstructionYard(mapTileInstance) && !mapTileInstance.IsBlockingTerrain && !GDIConstructionYard.ContainsPoint(pointInWordlCoordinates);

        }

        private bool IsRelativeMapTileInstanceAdjacentToConstructionsYard(MapTileInstance mapTileInstance,
            TILE_LOCATION tileLocation)
        {
            MapTileInstance adjacentTile = FindAdjacentMapTileInstance(mapTileInstance, tileLocation);
            if (adjacentTile != null && GDIConstructionYard.ContainsPoint(PointUtil.ConvertVector2ToPoint(adjacentTile.PositionInWorldCoordinates)))
            {
                return true;
            }

            return false;

        }


        enum TILE_LOCATION
        {
            WEST,
            NORTH_WEST,
            NORTH,
            NORTH_EAST,
            EAST,
            SOUTH_EAST,
            SOUTH,
            SOUTH_WEST
        }



        private bool IsMapTileInstanceAdjacentToConstructionYard(MapTileInstance mapTileInstance)
        {

            if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.WEST))
            {
                return true;
            }

            if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.NORTH_WEST))
            {
                return true;
            }

            if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.NORTH))
            {
                return true;
            }

            if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.NORTH_EAST))
            {
                return true;
            }

            if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.EAST))
            {
                return true;
            }

            if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.SOUTH_EAST))
            {
                return true;
            }

            if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.SOUTH))
            {
                return true;
            }

            if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.SOUTH_WEST))
            {
                return true;
            }

            return false;

        }




        private MapTileInstance FindAdjacentMapTileInstance(MapTileInstance mapTileInstance,TILE_LOCATION tileLocation)
        {

            Point adjacentTilePositionMapTileCoordinates = this.ConvertWorldCoordinatesToMapTileCoordinates( PointUtil.ConvertVector2ToPoint(mapTileInstance.PositionInWorldCoordinates));
            if (tileLocation == TILE_LOCATION.WEST)
            {
                adjacentTilePositionMapTileCoordinates.X = adjacentTilePositionMapTileCoordinates.X - 1;
            }
            else if (tileLocation == TILE_LOCATION.NORTH_WEST)
            {
                adjacentTilePositionMapTileCoordinates.X = adjacentTilePositionMapTileCoordinates.X - 1;
                adjacentTilePositionMapTileCoordinates.Y = adjacentTilePositionMapTileCoordinates.Y - 1;
            }
            else if (tileLocation == TILE_LOCATION.NORTH)
            {
                adjacentTilePositionMapTileCoordinates.Y = adjacentTilePositionMapTileCoordinates.Y - 1;
            }
            else if (tileLocation == TILE_LOCATION.NORTH_EAST)
            {
                adjacentTilePositionMapTileCoordinates.X = adjacentTilePositionMapTileCoordinates.X + 1;
                adjacentTilePositionMapTileCoordinates.Y = adjacentTilePositionMapTileCoordinates.Y - 1;
            }
            else if (tileLocation == TILE_LOCATION.EAST)
            {
                adjacentTilePositionMapTileCoordinates.X = adjacentTilePositionMapTileCoordinates.X + 1;
            }
            else if (tileLocation == TILE_LOCATION.SOUTH_EAST)
            {
                adjacentTilePositionMapTileCoordinates.X = adjacentTilePositionMapTileCoordinates.X + 1;
                adjacentTilePositionMapTileCoordinates.Y = adjacentTilePositionMapTileCoordinates.Y + 1;
            }
            else if (tileLocation == TILE_LOCATION.SOUTH)
            {
                adjacentTilePositionMapTileCoordinates.Y = adjacentTilePositionMapTileCoordinates.Y + 1;
            }
            else if (tileLocation == TILE_LOCATION.SOUTH_WEST)
            {
                adjacentTilePositionMapTileCoordinates.X = adjacentTilePositionMapTileCoordinates.X - 1;
                adjacentTilePositionMapTileCoordinates.Y = adjacentTilePositionMapTileCoordinates.Y + 1;
            }

            Point adjacentTilePositionInWorldCoordinates =
                ConvertMapTileCoordinatesToWorldCoordinates(adjacentTilePositionMapTileCoordinates);
            MapTileInstance fouMapTileInstance =
                this.FindMapTileInstanceAllowNull(adjacentTilePositionInWorldCoordinates.X,
                    adjacentTilePositionInWorldCoordinates.Y);

            return fouMapTileInstance;

        }

        public  bool IsValidMoveDestination(Point pointInWorldCoordinates)
        {

            Boolean isValidMoveDestination = true;
            MapTileInstance clickedMapTileInstance =
                FindMapTileInstanceAllowNull(pointInWorldCoordinates.X, pointInWorldCoordinates.Y);
            if (clickedMapTileInstance == null)
            {
                isValidMoveDestination = false;
            }
            else if (clickedMapTileInstance.IsBlockingTerrain)
            {
                isValidMoveDestination = false;
            }


            foreach (Sandbag nextSandbag in MikeAndConquerGame.instance.gameWorld.sandbagList)
            {

                if (nextSandbag.ContainsPoint(pointInWorldCoordinates))
                {
                    isValidMoveDestination = false;
                }
            }

            return isValidMoveDestination;
        }

        public bool IsPointOverEnemy(Point pointInWorldCoordinates)
        {
            // foreach (Minigunner nextNodMinigunner in nodMinigunnerList)
            // {
            //     if (nextNodMinigunner.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
            //     {
            //         return true;
            //     }
            // }
            //
            // return false;
            //
            return nodPlayer.IsPointOverMinigunner(pointInWorldCoordinates);
        }

        public bool IsPointOverMCV(Point pointInWorldCoordinates)
        {

            // if (this.mcv != null)
            // {
            //     if (mcv.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
            //     {
            //         return true;
            //     }
            // }
            //
            // return false;

            return gdiPlayer.IsPointOverMCV(pointInWorldCoordinates);
        }


        public bool IsAMinigunnerSelected()
        {
            // foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            // {
            //     if (nextMinigunner.selected)
            //     {
            //         return true;
            //     }
            // }
            // return false;
            return gdiPlayer.IsAMinigunnerSelected();
        }

        public bool IsAnMCVSelected()
        {
            // if (mcv != null)
            // {
            //     return mcv.selected;
            // }
            //
            // return false;
            return gdiPlayer.IsAnMCVSelected();
        }

        public bool IsAnyUnitSelected()
        {
            return IsAMinigunnerSelected() || IsAnMCVSelected();
        }


        public void RemoveMCV()
        {
            gdiPlayer.RemoveMCV();
        }
    }
}
