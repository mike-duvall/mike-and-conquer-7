using System;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameview;
using mike_and_conquer.gameview.sidebar;
using mike_and_conquer.openralocal;
using mike_and_conquer.src.externalcontrol;
using Serilog;
using Game = Microsoft.Xna.Framework.Game;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;

using GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile;
using Color = Microsoft.Xna.Framework.Color;

using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using GdiMinigunnerView = mike_and_conquer.gameview.GdiMinigunnerView;
using NodMinigunnerView = mike_and_conquer.gameview.NodMinigunnerView;
using SandbagView = mike_and_conquer.gameview.SandbagView;


using Point = Microsoft.Xna.Framework.Point;
using MCVSelectionBox = mike_and_conquer.gameview.MCVSelectionBox;




namespace mike_and_conquer.main
{

    public class MikeAndConquerGame : Game
    {

        private float testRotation = 0;

        public static MikeAndConquerGame instance;

        public GameWorld gameWorld;
        private GameWorldView gameWorldView;

        private GameStateView currentGameStateView;

        public UnitSelectionBox unitSelectionBox;

        private GameState currentGameState;

        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
        }

        private RAISpriteFrameManager raiSpriteFrameManager;
        private SpriteSheet spriteSheet;

        private GraphicsDeviceManager graphics;
        private bool testMode;

        private int mouseCounter = 0;

        KeyboardState oldKeyboardState;

        public Serilog.Core.Logger log = new LoggerConfiguration()
            //.WriteTo.Console()
            //.WriteTo.File("log.txt")
            .WriteTo.Debug()
            .CreateLogger();

        public const string CONTENT_DIRECTORY_PREFIX = "Content\\";

        public MikeAndConquerGame(bool testMode)
        {
            RemoveHostingTraceListenerToEliminateDuplicateLogEntries();

            log.Information("Hello, Serilog!");

            this.testMode = testMode;
            graphics = new GraphicsDeviceManager(this);

            if (GameOptions.IS_FULL_SCREEN)
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                graphics.IsFullScreen = false;
//                graphics.PreferredBackBufferWidth = 1280;
//                graphics.PreferredBackBufferHeight = 1024;
                graphics.PreferredBackBufferWidth = 1024;
                graphics.PreferredBackBufferHeight = 768;


            }

            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.IsFixedTimeStep = false;

            gameWorld = new GameWorld();
            gameWorldView = new GameWorldView();

            oldKeyboardState = Keyboard.GetState();
            unitSelectionBox = new UnitSelectionBox();


            // shadowMapper = new ShadowMapper();
            currentGameState = new PlayingGameState();

            raiSpriteFrameManager = new RAISpriteFrameManager();
            spriteSheet = new SpriteSheet();

            MikeAndConquerGame.instance = this;
            // redrawBaseMapTiles = true;
        }


        private void RemoveHostingTraceListenerToEliminateDuplicateLogEntries()
        {
            System.Diagnostics.Trace.Listeners.Remove("HostingTraceListener");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.InitializeDefaultMap will enumerate through any components
        /// and initialize them as well.
        /// </summary>
//        protected override void Initialize()
//        {
//            // TODO: Add your initialization logic here
//            base.InitializeDefaultMap();
//        }


        private void AddTestModeObjects()
        {
            bool aiIsOn = false;


            if (!GameOptions.IS_FULL_SCREEN)
            {
                AddGdiMinigunnerAtMapSquareCoordinates(new Point(21, 11));
                AddMCVAtMapSquareCoordinates(new Point(21, 12));
            }


            // AddGdiMinigunnerAtMapSquareCoordinates(new Point(21, 11));
            // AddMCVAtMapSquareCoordinates(new Point(21, 12));


            //
            //            AddSandbag(10, 6, 5);
            //            AddSandbag(10, 7, 5);
            //            AddSandbag(10, 8, 5);
            //            AddSandbag(10, 9, 5);
            //            AddSandbag(10, 10, 5);
            //
            //            AddSandbag(8, 4, 10);
            //            AddSandbag(9, 4, 10);

            //                AddSandbag(12, 16, 10);

            //                AddSandbag(11, 16, 2);
            //                AddSandbag(12, 16, 8);
            //
            //
            //                AddSandbag(14, 5, 0);
            //                AddSandbag(14, 6, 2);
            //                AddSandbag(14, 7, 8);
//            minigunnerSidebarIconView = new MinigunnerSidebarIconView();
//            AddGDIBarracksAtMapSquareCoordinates(new Point(20, 15));

        }


        private void CreateBasicMapSquareViews()
        {
            foreach(MapTileInstance mapTileInstance in this.gameWorld.gameMap.MapTileInstanceList)
            {
                gameWorldView.AddMapTileInstanceView(mapTileInstance);
            }
        }

        private void CreateTerrainItemViews()
        {
            foreach (TerrainItem terrainItem in gameWorld.terrainItemList)
            {
                gameWorldView.AddTerrainItemView(terrainItem);
            }
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            gameWorld.InitializeDefaultMap();

            // spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();
            CreateBasicMapSquareViews();
            CreateTerrainItemViews();

            if (!testMode)
            {
                AddTestModeObjects();
            }

            //AddGDIConstructionYardAtMapTileCoordinates(new Point(20, 15));

            gameWorld.InitializeNavigationGraph();
            gameWorldView.LoadContent();
        }

        private void LoadTextures()
        {
            LoadMapTextures();
            LoadSingleTextures();
            LoadShpFileTextures();
            LoadTemFiles();
        }

        private void LoadShpFileTextures()
        {
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GdiMinigunnerView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GdiMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GdiMinigunnerView.SHP_FILE_NAME),
                GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                NodMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(NodMinigunnerView.SHP_FILE_NAME),
                NodMinigunnerView.SHP_FILE_COLOR_MAPPER);


            raiSpriteFrameManager.LoadAllTexturesFromShpFile(MCVView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                MCVView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(MCVView.SHP_FILE_NAME),
                MCVView.SHP_FILE_COLOR_MAPPER);



            raiSpriteFrameManager.LoadAllTexturesFromShpFile(SandbagView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                SandbagView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(SandbagView.SHP_FILE_NAME),
                SandbagView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(MinigunnerSidebarIconView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                MinigunnerSidebarIconView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(MinigunnerSidebarIconView.SHP_FILE_NAME),
                MinigunnerSidebarIconView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(BarracksSidebarIconView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                BarracksSidebarIconView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(BarracksSidebarIconView.SHP_FILE_NAME),
                BarracksSidebarIconView.SHP_FILE_COLOR_MAPPER);



            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIBarracksView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GDIBarracksView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GDIBarracksView.SHP_FILE_NAME),
                GDIBarracksView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIConstructionYardView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GDIConstructionYardView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GDIConstructionYardView.SHP_FILE_NAME),
                GDIConstructionYardView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(PartiallyVisibileMapTileMask.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(PartiallyVisibileMapTileMask.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(PartiallyVisibileMapTileMask.SHP_FILE_NAME),
                GDIBarracksView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(UnitSelectionCursor.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                UnitSelectionCursor.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(UnitSelectionCursor.SHP_FILE_NAME),
                UnitSelectionCursor.SHP_FILE_COLOR_MAPPER);

        }


        private void LoadTemFiles()
        {
            LoadTerrainTexture("T01.tem");
            LoadTerrainTexture("T02.tem");
            LoadTerrainTexture("T05.tem");
            LoadTerrainTexture("T06.tem");
            LoadTerrainTexture("T07.tem");
            LoadTerrainTexture("T16.tem");
            LoadTerrainTexture("T17.tem");
            LoadTerrainTexture("TC01.tem");
            LoadTerrainTexture("TC02.tem");
            LoadTerrainTexture("TC04.tem");
            LoadTerrainTexture("TC05.tem");

        }


        private void LoadTerrainTexture(String filename)
        {
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(filename);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                filename,
                raiSpriteFrameManager.GetSpriteFramesForUnit(filename),
                TerrainView.SHP_FILE_COLOR_MAPPER);

        }

        private void LoadSingleTextures()
        {
            spriteSheet.LoadSingleTextureFromFile(MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            spriteSheet.LoadSingleTextureFromFile(MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY, "Accomplished");
            spriteSheet.LoadSingleTextureFromFile(MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");
            spriteSheet.LoadSingleTextureFromFile(DestinationSquare.SPRITE_KEY, DestinationSquare.SPRITE_KEY);
            spriteSheet.LoadSingleTextureFromFile(MCVSelectionBox.SPRITE_KEY, MCVSelectionBox.SPRITE_KEY);
        }



        private void LoadTmpFile(string tmpFileName)
        {
            raiSpriteFrameManager.LoadAllTexturesFromTmpFile(tmpFileName);
            spriteSheet.LoadMapTileFramesFromSpriteFrames(
                tmpFileName,
                raiSpriteFrameManager.GetSpriteFramesForMapTile(tmpFileName));

        }

        private void LoadMapTextures()
        {
            LoadTmpFile(GameMap.CLEAR1_SHP);
            LoadTmpFile(GameMap.D04_TEM);
            LoadTmpFile(GameMap.D09_TEM);
            LoadTmpFile(GameMap.D13_TEM);
            LoadTmpFile(GameMap.D15_TEM);
            LoadTmpFile(GameMap.D20_TEM);
            LoadTmpFile(GameMap.D21_TEM);
            LoadTmpFile(GameMap.D23_TEM);

            LoadTmpFile(GameMap.P07_TEM);
            LoadTmpFile(GameMap.P08_TEM);

            LoadTmpFile(GameMap.S09_TEM);
            LoadTmpFile(GameMap.S10_TEM);
            LoadTmpFile(GameMap.S11_TEM);
            LoadTmpFile(GameMap.S12_TEM);
            LoadTmpFile(GameMap.S14_TEM);
            LoadTmpFile(GameMap.S22_TEM);
            LoadTmpFile(GameMap.S29_TEM);
            LoadTmpFile(GameMap.S32_TEM);
            LoadTmpFile(GameMap.S34_TEM);
            LoadTmpFile(GameMap.S35_TEM);

            LoadTmpFile(GameMap.SH1_TEM);
            LoadTmpFile(GameMap.SH2_TEM);
            LoadTmpFile(GameMap.SH3_TEM);
            LoadTmpFile(GameMap.SH4_TEM);
            LoadTmpFile(GameMap.SH5_TEM);
            LoadTmpFile(GameMap.SH6_TEM);
            LoadTmpFile(GameMap.SH9_TEM);
            LoadTmpFile(GameMap.SH10_TEM);
            LoadTmpFile(GameMap.SH17_TEM);
            LoadTmpFile(GameMap.SH18_TEM);

            LoadTmpFile(GameMap.W1_TEM);
            LoadTmpFile(GameMap.W2_TEM);


        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }






        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


//            log.Information("gameTime.ElapsedGameTime.TotalMilliseconds:" + gameTime.ElapsedGameTime.TotalMilliseconds);

            KeyboardState newKeyboardState = Keyboard.GetState();

            // If they hit esc, exit

            if (newKeyboardState.IsKeyDown(Keys.Escape))
            {
                RestServerManager.Shutdown();
                Exit();
            }

            gameWorldView.Update(gameTime, newKeyboardState);


            currentGameState = this.currentGameState.Update(gameTime);
            this.currentGameStateView.Update(gameTime);

            // This is a hack fix to fix an issue where if you change this.IsMouseVisible to false
            // while the Windows pointer is showing the mouse pointer arrow with the blue sworl "busy" icon on the side
            // it will continue to show a frozen(non moving) copy of the blue sworl "busy" icon, even after it 
            // stops showing and updating the normal Winodws mouse pointer (in favor of my manually handled one)
            // TODO:  Investigate replacing countdown timer with direct call to (possibly to native Windows API) to determine
            // native mouse pointer "busy" status, and wait until ti goes "not busy"
            if (mouseCounter < 20)
            {
                this.IsMouseVisible = true;
                mouseCounter++;
            }
            else
            {
                this.IsMouseVisible = false;
            }


            base.Update(gameTime);
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }



        public void SwitchToNewGameStateViewIfNeeded()
        {
            GameState currentGameState = this.GetCurrentGameState();
            if (currentGameState.GetType().Equals(typeof(PlayingGameState)))
            {
                HandleSwitchToPlayingGameStateView();
            }
            else if (currentGameState.GetType().Equals(typeof(MissionAccomplishedGameState)))
            {
                HandleSwitchToMissionAccomplishedGameStateView();
            }
            else if (currentGameState.GetType().Equals(typeof(MissionFailedGameState)))
            {
                HandleSwitchToMissionFailedGameStateView();
            }
        }

        private void HandleSwitchToPlayingGameStateView()
        {
            if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(PlayingGameStateView)))
            {
                currentGameStateView = new PlayingGameStateView();
            }
        }

        private void HandleSwitchToMissionAccomplishedGameStateView()
        {
            if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(MissionAccomplishedGameStateView)))
            {
                currentGameStateView = new MissionAccomplishedGameStateView();
            }
        }

        private void HandleSwitchToMissionFailedGameStateView()
        {
            if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(MissionFailedGameStateView)))
            {
                currentGameStateView = new MissionFailedGameStateView();
            }
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // long amountOfMemory = GC.GetTotalMemory(false);
            // amountOfMemory = GC.GetTotalMemory(true);
            Viewport originalViewport = GraphicsDevice.Viewport;

            GraphicsDevice.Clear(Color.Crimson);

            currentGameStateView.Draw(gameTime);
            //
            // DrawMap(gameTime);
            // DrawSidebar(gameTime);
            // DrawGameCursor(gameTime);

            // GraphicsDevice.Viewport = defaultViewport;
            GraphicsDevice.Viewport = originalViewport;
            base.Draw(gameTime);


        }




        internal Minigunner AddGdiMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {

            Point positionInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);
            return AddGdiMinigunner(positionInWorldCoordinates);
        }

        // internal void MakeMapSquareVisibleAtMapSquareCoorindates(Point positionInMapSquareCoordinates, MapTileInstance.MapTileVisibility visibility)
        // {
        //
        //     Point positionInWorldCoordinates =
        //         gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);
        //
        //     gameWorld.MakeMapSquareVisible(positionInWorldCoordinates, visibility);
        //
        // }

        internal Minigunner AddGdiMinigunner(Point positionInWorldCoordinates)
        {

            Minigunner newMinigunner =  gameWorld.AddGdiMinigunner(positionInWorldCoordinates);

            // TODO:  In future, decouple always adding a view when adding a minigunner
            // to enable running headless with no UI
            gameWorldView.AddGDIMinigunnerView(newMinigunner);

            return newMinigunner;
        }

        internal Sandbag AddSandbag(int xInMapSquareCoordinates, int yInMapSquareCoordinates, int sandbagType)
        {

            int xInWorldCoordinates =
                (xInMapSquareCoordinates * GameWorld.MAP_TILE_WIDTH) + (GameWorld.MAP_TILE_WIDTH / 2);

            int yInWorldCoordinates =
                (yInMapSquareCoordinates * GameWorld.MAP_TILE_HEIGHT) + (GameWorld.MAP_TILE_HEIGHT / 2);

            Sandbag newSandbag = new Sandbag(xInWorldCoordinates, yInWorldCoordinates, sandbagType);
            gameWorld.sandbagList.Add(newSandbag);

            gameWorldView.AddSandbagView(newSandbag);

            return newSandbag;
        }


        public void AddGDIBarracksAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            int xInWorldCoordinates = positionInMapSquareCoordinates.X * GameWorld.MAP_TILE_WIDTH;
            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * GameWorld.MAP_TILE_HEIGHT;

            Point positionInWorldCoordinates = new Point(xInWorldCoordinates, yInWorldCoordinates);

            AddGDIBarracksAtWorldCoordinates(positionInWorldCoordinates);
        }


        public void AddGDIBarracksAtWorldCoordinates(Point positionInWorldCoordinates)
        {
            GDIBarracks gdiBarracks = gameWorld.AddGDIBarracks(positionInWorldCoordinates);
            gameWorldView.AddGDIBarracksView(gdiBarracks);
        }


        public void AddGDIConstructionYardAtWorldCoordinates(Point positionInWorldCoordinates)
        {
            GDIConstructionYard gdiConstructionYard = gameWorld.AddGDIConstructionYard(positionInWorldCoordinates);
            gameWorldView.AddGDIConstructionYardView(gdiConstructionYard);
        }


        public void AddMCVAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            Point positionInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            MCV mcv = gameWorld.AddMCV(positionInWorldCoordinates);
            gameWorldView.AddMCVView(mcv);
        }

        public MCV AddMCVAtWorldCoordinates(Point positionInWorldCoordinates)
        {
            MCV mcv = gameWorld.AddMCV(positionInWorldCoordinates);
            gameWorldView.AddMCVView(mcv);
            return mcv;
        }


        public void RemoveMCV()
        {
            GameWorld.instance.MCV = null;
            GameWorldView.instance.mcvView = null;

        }


        internal Minigunner AddNodMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates, bool aiIsOn)
        {

            Point positionInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            return AddNodMinigunner(positionInWorldCoordinates, aiIsOn);
        }


        internal Minigunner AddNodMinigunner(Point positionInWorldCoordinates, bool aiIsOn)
        {
            Minigunner newMinigunner = gameWorld.AddNodMinigunner(positionInWorldCoordinates, aiIsOn);
            gameWorldView.AddNodMinigunnerView(newMinigunner);

            return newMinigunner;
        }

        public GameState HandleReset(bool drawShroud)
        {
            GameOptions.DRAW_SHROUD = drawShroud;
            GameState newGameState = gameWorld.HandleReset();
            gameWorldView.HandleReset();
            return newGameState;
        }

    }




}
