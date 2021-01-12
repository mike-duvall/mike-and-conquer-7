using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.externalcontrol;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameview;
using mike_and_conquer.gameview.sidebar;
using mike_and_conquer.gameworld;
using mike_and_conquer.openralocal;
using mike_and_conquer.sound;
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


        //        private float testRotation = 0;

        public static MikeAndConquerGame instance;

        public GameWorld gameWorld;
        private GameWorldView gameWorldView;

        private GameStateView currentGameStateView;


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

        public Serilog.Core.Logger log = new LoggerConfiguration()
             //.WriteTo.Console()
             //.WriteTo.File("log.txt")
            .WriteTo.Debug(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}]  {Message}{NewLine}{Exception}")
            .CreateLogger();


        public const string CONTENT_DIRECTORY_PREFIX = "Content\\";

        private SoundManager soundManager;

        public SoundManager SoundManager
        {
            get { return soundManager; }
        }

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

            currentGameState = new PlayingGameState();

            raiSpriteFrameManager = new RAISpriteFrameManager();
            spriteSheet = new SpriteSheet();

            soundManager = new SoundManager();

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
            //            bool aiIsOn = false;



            // AddGdiMinigunnerAtMapSquareCoordinates(new Point(16, 9));
            // AddMCVAtMapSquareCoordinates(new Point(21, 12));
            // AddNodTurret(MapTileLocation.CreateFromWorldMapTileCoordinates(14, 16), 90, 2);


            if (!GameOptions.IS_FULL_SCREEN)
            {
                AddGdiMinigunnerAtMapSquareCoordinates(new Point(21, 9));
                AddMCVAtMapSquareCoordinates(new Point(21, 12));
                AddNodTurret(MapTileLocation.CreateFromWorldMapTileCoordinates(14, 16), 90, 2);
            }


//            AddProjectile120mmAtGameWorldLocation(GameWorldLocation.CreateFromWorldCoordinates(200,200));

            //            AddGdiMinigunnerAtMapSquareCoordinates(new Point(21, 11));
            //                        AddMCVAtMapSquareCoordinates(new Point(21, 12));
            //            AddNodMinigunnerAtMapSquareCoordinates(new Point(20,11),false);


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


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            gameWorld.InitializeDefaultMap();

            // spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();

            if (!testMode)
            {
                AddTestModeObjects();
            }

            //AddGDIConstructionYardAtMapTileCoordinates(new Point(20, 15));

            gameWorld.InitializeNavigationGraph();
            gameWorldView.LoadContent();


            soundManager.LoadContent(Content);

            soundManager.PlaySong();
        }


        private void LoadTextures()
        {
            LoadMapTextures();
            LoadSingleTextures();
            LoadShpFileTextures();
            LoadTemFiles();
            LoadBarracksPlacementTexture();
        }

        private void LoadBarracksPlacementTexture()
        {
            LoadTmpFile(BarracksPlacementIndicatorView.FILE_NAME);
            MapBlackMapTileFramePixelsToToTransparent(BarracksPlacementIndicatorView.FILE_NAME);
        }


        private void MapBlackMapTileFramePixelsToToTransparent(string tmpFileName)
        {
            List<MapTileFrame> mapTileFrameList = spriteSheet.GetMapTileFrameForTmpFile(tmpFileName);
            foreach (MapTileFrame mapTileFrame in mapTileFrameList)
            {
                Texture2D theTexture = mapTileFrame.Texture;
                int numPixels = theTexture.Height * theTexture.Width;
                Color[] originalTexturePixelData = new Color[numPixels];
                Color[] changedTexturePixelData = new Color[numPixels];
                theTexture.GetData(originalTexturePixelData);

                int i = 0;
                foreach (Color color in originalTexturePixelData)
                {
                    if (color.R == 0)
                    {
                        Color newColor = new Color(0, 0, 0, 0);
                        changedTexturePixelData[i] = newColor;
                    }
                    else
                    {
                        changedTexturePixelData[i] = color;
                    }

                    i++;
                }
                theTexture.SetData(changedTexturePixelData);

            }

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

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(NodTurretView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                NodTurretView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(NodTurretView.SHP_FILE_NAME),
                NodTurretView.SHP_FILE_COLOR_MAPPER);


            raiSpriteFrameManager.LoadAllTexturesFromShpFile(Projectile120mmView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                Projectile120mmView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(Projectile120mmView.SHP_FILE_NAME),
                Projectile120mmView.SHP_FILE_COLOR_MAPPER);


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
            spriteSheet.LoadSingleTextureFromFile(ReadyOverlay.SPRITE_KEY, ReadyOverlay.SPRITE_KEY);

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


        private KeyboardState oldKeyboardState;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //      log.Information("gameTime.ElapsedGameTime.TotalMilliseconds:" + gameTime.ElapsedGameTime.TotalMilliseconds);

            FixMousePointerProblem();

            KeyboardState newKeyboardState = Keyboard.GetState();
            ExitIfEscPressed(newKeyboardState);

//            if (!oldKeyboardState.IsKeyDown(Keys.Q) && newKeyboardState.IsKeyDown(Keys.Q))
//            {
//                Projectile120mm.movementVelocity += 0.01;
//            }
//            else if (!oldKeyboardState.IsKeyDown(Keys.A) && newKeyboardState.IsKeyDown(Keys.A))
//            {
//                Projectile120mm.movementVelocity -= 0.01;
//            }


            gameWorldView.Update(gameTime, newKeyboardState);

            currentGameState = this.currentGameState.Update(gameTime);
            this.currentGameStateView.Update(gameTime);

            oldKeyboardState = newKeyboardState;
            base.Update(gameTime);
        }

        private void ExitIfEscPressed(KeyboardState newKeyboardState)
        {
            // If they hit esc, exit
            if (newKeyboardState.IsKeyDown(Keys.Escape))
            {
                RestServerManager.Shutdown();
                Exit();
            }
        }


        private void FixMousePointerProblem()
        {
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
                MapTileLocation.ConvertMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);
            return AddGdiMinigunner(positionInWorldCoordinates);
        }

        // internal void MakeMapSquareVisibleAtMapSquareCoorindates(Point positionInMapSquareCoordinates, MapTileInstance.MapTileVisibility visibility)
        // {
        //
        //     Point positionInWorldCoordinates =
        //         gameWorld.ConvertMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);
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


        public NodTurret AddNodTurret(MapTileLocation mapTileLocation, float direction, int nodTurretType)
        {
            //            NodTurret newNodTurret =
            //                new NodTurret(mapTileLocation, nodTurretType, 90.0f - NodTurret.TURN_ANGLE_SIZE);

            NodTurret newNodTurret =
                new NodTurret(mapTileLocation, nodTurretType, direction);

            gameWorld.nodTurretList.Add(newNodTurret);
            gameWorldView.AddNodTurretView(newNodTurret);
            return newNodTurret;

        }

        internal Sandbag AddSandbag(MapTileLocation mapTileLocation, int sandbagType)
        {
            Sandbag newSandbag = new Sandbag(mapTileLocation, sandbagType);
            gameWorld.sandbagList.Add(newSandbag);

            gameWorldView.AddSandbagView(newSandbag);

            return newSandbag;
        }



        public void AddGDIBarracks(MapTileLocation mapTileLocation)
        {
            GDIBarracks gdiBarracks = gameWorld.AddGDIBarracks(mapTileLocation);
            gameWorldView.AddGDIBarracksView(gdiBarracks);
        }

        public void AddGDIConstructionYard(MapTileLocation mapTileLocation)
        {
            GDIConstructionYard gdiConstructionYard = gameWorld.AddGDIConstructionYard(mapTileLocation);
            gameWorldView.AddGDIConstructionYardView(gdiConstructionYard);
        }


        public void AddMCVAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            Point positionInWorldCoordinates =
                MapTileLocation.ConvertMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            MCV mcv = gameWorld.AddMCV(positionInWorldCoordinates);
            gameWorldView.AddMCVView(mcv);
        }

        public void AddProjectile120mmAtGameWorldLocation(GameWorldLocation gameWorldLocation, GameWorldLocation targetLocation)
        {
            Projectile120mm projectile120Mm = gameWorld.AddProjectile120mm(gameWorldLocation, targetLocation);
            gameWorldView.AddProjectile120mmView(projectile120Mm);
        }

        public void RemoveProjectile120mmWithId(int id)
        {
            gameWorldView.RemoveProjectile120mmView(id);
            gameWorld.RemoveProjectile120mm(id);
        }


        public MCV AddMCVAtWorldCoordinates(Point positionInWorldCoordinates)
        {
            MCV mcv = gameWorld.AddMCV(positionInWorldCoordinates);
            gameWorldView.AddMCVView(mcv);
            return mcv;
        }


        public void RemoveMCV()
        {
            GameWorld.instance.RemoveMCV();
            GameWorldView.instance.mcvView = null;
        }



        internal Minigunner AddNodMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates, bool aiIsOn)
        {

            Point positionInWorldCoordinates =
                MapTileLocation.ConvertMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

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
