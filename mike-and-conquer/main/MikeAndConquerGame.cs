
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gameview;
using mike_and_conquer.openralocal;
using Game = Microsoft.Xna.Framework.Game;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile;
using Color = Microsoft.Xna.Framework.Color;

using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using MinigunnerView = mike_and_conquer.gameview.MinigunnerView;
using GdiMinigunnerView = mike_and_conquer.gameview.GdiMinigunnerView;
using NodMinigunnerView = mike_and_conquer.gameview.NodMinigunnerView;
using SandbagView = mike_and_conquer.gameview.SandbagView;
using Camera2D = mike_and_conquer_6.Camera2D;

using Point = Microsoft.Xna.Framework.Point;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Serilog;

using Matrix = Microsoft.Xna.Framework.Matrix;




namespace mike_and_conquer
{

    public class MikeAndConquerGame : Game
    {


        public Viewport defaultViewport;
        private Viewport mapViewport;
        private Viewport toolbarViewport;
        private float testRotation = 0;
        public Camera2D mapViewportCamera;
        private Camera2D toolbarViewportCamera;

        public static MikeAndConquerGame instance;

        public GameWorld gameWorld;

        public ShadowMapper shadowMapper;
        private List<MinigunnerView> gdiMinigunnerViewList;
        private List<MinigunnerView> nodMinigunnerViewList;

        private GDIBarracksView gdiBarracksView;
        private MinigunnerIconView minigunnerIconView;

        private List<MapTileInstanceView> mapTileInstanceViewList;

        private List<SandbagView> sandbagViewList;

        private GameStateView currentGameStateView;

        public GameCursor gameCursor;

        public UnitSelectionBox unitSelectionBox;

        private GameState currentGameState;

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

        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
        }

        private RAISpriteFrameManager raiSpriteFrameManager;
        private SpriteSheet spriteSheet;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private bool testMode;

        private int borderSize = 0;

        private int mouseCounter = 0;

        KeyboardState oldKeyboardState;

        public Serilog.Core.Logger log = new LoggerConfiguration()
            //.WriteTo.Console()
            //.WriteTo.File("log.txt")
            .WriteTo.Debug()
            .CreateLogger();


        public MikeAndConquerGame(bool testMode)
        {
            RemoveHostingTraceListenerToEliminateDuplicateLogEntries();

            log.Information("Hello, Serilog!");

            this.testMode = testMode;
            graphics = new GraphicsDeviceManager(this);

            bool makeFullscreen = true;
//            bool makeFullscreen = false;
            if (makeFullscreen)
            {
                graphics.IsFullScreen = true;
//                graphics.PreferredBackBufferWidth = 1920;
//                graphics.PreferredBackBufferHeight = 1080;
                graphics.PreferredBackBufferWidth = 2880;
                graphics.PreferredBackBufferHeight = 1800;

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

            mapTileInstanceViewList = new List<MapTileInstanceView>();

            gdiMinigunnerViewList = new List<MinigunnerView>();
            nodMinigunnerViewList = new List<MinigunnerView>();

            sandbagViewList = new List<SandbagView>();

            oldKeyboardState = Keyboard.GetState();
            unitSelectionBox = new UnitSelectionBox();

            shadowMapper = new ShadowMapper();
            currentGameState = new PlayingGameState();

            raiSpriteFrameManager = new RAISpriteFrameManager();
            spriteSheet = new SpriteSheet();

            MikeAndConquerGame.instance = this;
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
//        protected override void InitializeDefaultMap()
//        {
//            // TODO: Add your initialization logic here
//            base.InitializeDefaultMap();
//        }

        private void AddTestModeObjects()
        {
            bool aiIsOn = false;

            AddGdiMinigunnerAtMapSquareCoordinates(new Point(6, 1));

            AddGdiMinigunnerAtMapSquareCoordinates(new Point(8, 3));

            AddNodMinigunnerAtMapSquareCoordinates(new Point(10, 3), aiIsOn);

            AddSandbag(10, 6, 5);
            AddSandbag(10, 7, 5);
            AddSandbag(10, 8, 5);
            AddSandbag(10, 9, 5);
            AddSandbag(10, 10, 5);

            AddSandbag(8, 4, 10);
            AddSandbag(9, 4, 10);

            //                AddSandbag(12, 16, 10);

            //                AddSandbag(11, 16, 2);
            //                AddSandbag(12, 16, 8);
            //
            //
            //                AddSandbag(14, 5, 0);
            //                AddSandbag(14, 6, 2);
            //                AddSandbag(14, 7, 8);
            gdiBarracksView = AddGDIBarracksViewAtMapSquareCoordinates(new Point(23, 15));
            minigunnerIconView = new MinigunnerIconView();
        }

        private void SetupToolbarViewportAndCamera()
        {
            toolbarViewport = new Viewport();
            toolbarViewport.X = mapViewport.Width + 2;
            toolbarViewport.Y = 0;
            toolbarViewport.Width = defaultViewport.Width - mapViewport.Width - 5;
            toolbarViewport.Height = defaultViewport.Height;
            toolbarViewport.MinDepth = 0;
            toolbarViewport.MaxDepth = 1;

            toolbarViewportCamera = new Camera2D(toolbarViewport);
            toolbarViewportCamera.Zoom = 3.0f;
            toolbarViewportCamera.Location = new Vector2(0, 0);
        }

        private void SetupMapViewportAndCamera()
        {
            mapViewport = new Viewport();
            mapViewport.X = 0;
            mapViewport.Y = 0;
            mapViewport.Width = (int) (defaultViewport.Width * 0.8f);
            mapViewport.Height = defaultViewport.Height;
            mapViewport.MinDepth = 0;
            mapViewport.MaxDepth = 1;

            this.mapViewportCamera = new Camera2D(mapViewport);
            this.mapViewportCamera.Zoom = 3.0f;
            this.mapViewportCamera.Location =
                new Vector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());
        }


        private void CreateBasicMapSquareViews()
        {
            //  (Starting at 0x13CC in the file)
            //    Trees appear to be SHP vs TMP?
            //    Map file only references TMP ?
            //    What about placement of initial troops?
            //    Sandbags

            foreach(MapTileInstance basicMapSquare in this.gameWorld.gameMap.MapTileInstanceList)
            {
                MapTileInstanceView mapTileInstanceView = new MapTileInstanceView(basicMapSquare);
                this.mapTileInstanceViewList.Add(mapTileInstanceView);
            }

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            gameWorld.InitializeDefaultMap();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();
            CreateBasicMapSquareViews();

            if (!testMode)
            {
                AddTestModeObjects();
            }

            gameWorld.InitializeNavigationGraph();
            gameCursor = new GameCursor(1, 1);

            this.defaultViewport = GraphicsDevice.Viewport;
            SetupMapViewportAndCamera();
            SetupToolbarViewportAndCamera();

        }


        private void LoadTextures()
        {
            LoadMapTextures();
            LoadSingleTextures();
            LoadShpFileTextures();
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


            raiSpriteFrameManager.LoadAllTexturesFromShpFile(SandbagView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                SandbagView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(SandbagView.SHP_FILE_NAME),
                SandbagView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(MinigunnerIconView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                MinigunnerIconView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(MinigunnerIconView.SHP_FILE_NAME),
                MinigunnerIconView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIBarracksView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GDIBarracksView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GDIBarracksView.SHP_FILE_NAME),
                GDIBarracksView.SHP_FILE_COLOR_MAPPER);
        }

        private void LoadSingleTextures()
        {
            spriteSheet.LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            spriteSheet.LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY,
                "Accomplished");
            spriteSheet.LoadSingleTextureFromFile(gameobjects.MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");
            spriteSheet.LoadSingleTextureFromFile(gameobjects.DestinationSquare.SPRITE_KEY,
                gameobjects.DestinationSquare.SPRITE_KEY);
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


        public float CalculateLeftmostScrollX()
        {
            int viewportWidth = mapViewport.Width;
            int halfViewportWidth = viewportWidth / 2;
            float scaledHalfViewportWidth = halfViewportWidth / mapViewportCamera.Zoom;
            return scaledHalfViewportWidth - borderSize;
        }

        private float CalculateRightmostScrollX()
        {
            int widthOfMapInWorldSpace = gameWorld.gameMap.numColumns * GameWorld.MAP_TILE_WIDTH;

            int viewportWidth = mapViewport.Width;
            int halfViewportWidth = viewportWidth / 2;

            float scaledHalfViewportWidth = halfViewportWidth / mapViewportCamera.Zoom;
            float amountToScrollHorizontally = widthOfMapInWorldSpace - scaledHalfViewportWidth;
            return amountToScrollHorizontally + borderSize;
        }

        public float CalculateTopmostScrollY()
        {
            int viewportHeight = mapViewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / mapViewportCamera.Zoom;
            return scaledHalfViewportHeight - borderSize;
        }

        private float CalculateBottommostScrollY()
        {
            int heightOfMapInWorldSpace = gameWorld.gameMap.numRows * GameWorld.MAP_TILE_HEIGHT;
            int viewportHeight = mapViewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / mapViewportCamera.Zoom;
            float amountToScrollVertically = heightOfMapInWorldSpace - scaledHalfViewportHeight;
            return amountToScrollVertically + borderSize;
        }


        private void SnapMapCameraToBounds()
        {
            float newX = this.mapViewportCamera.Location.X;
            float newY = this.mapViewportCamera.Location.Y;

            // TODO:  Consider if we store these as class variables
            // and only recalculate when the zoom changes
            float rightMostScrollX = CalculateRightmostScrollX();
            float leftMostScrollX = CalculateLeftmostScrollX();
            float topmostScrollY = CalculateTopmostScrollY();
            float bottommostScrollY = CalculateBottommostScrollY();
            if (newX > rightMostScrollX)
            {
                newX = rightMostScrollX;
            }
            if (newY > bottommostScrollY)
            {
                newY = bottommostScrollY;
            }

            // Check for leftmost and topmost last, which makes it snap to top left corner
            // if zoom is such that entire map fits on current screen
            if (newX < leftMostScrollX)
            {
                newX = leftMostScrollX;
            }

            if (newY < topmostScrollY)
            {
                newY = topmostScrollY;
            }

            this.mapViewportCamera.Location = new Vector2(newX, newY);

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


//            log.Information("gameTime.ElapsedGameTime.TotalMilliseconds:" + gameTime.ElapsedGameTime.TotalMilliseconds);

            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape))
            {
                Program.restServer.Dispose();
                Exit();
            }

            if (state.IsKeyDown(Keys.B))
            {
                borderSize = 1;
            }
            if (state.IsKeyDown(Keys.N))
            {
                borderSize = 0;
            }


            if (state.IsKeyDown(Keys.I))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());
            }
            if (state.IsKeyDown(Keys.P))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateRightmostScrollX(), CalculateTopmostScrollY());
            }
            if (state.IsKeyDown(Keys.M))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateLeftmostScrollX(), CalculateBottommostScrollY());
            }
            if (state.IsKeyDown(Keys.OemPeriod))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateRightmostScrollX(), CalculateBottommostScrollY());
            }

            currentGameState = this.currentGameState.Update(gameTime);
//            this.gameWorld.Update(gameTime);
            this.mapViewportCamera.Rotation = testRotation;
            //                        testRotation += 0.05f;



            // This is a hack fix to fix an issue where if you change this.IsMouseVisible to false
            // while the Windows pointer is showing the mouse pointer arrow with the blue sworl "busy" icon on the side
            // it will continue to show a frozen(non moving) copy of the blue sworl "busy" icon, even after it 
            // stops showing and updating the normal Winodws mouse pointer (in favor of my manually handled one)
            if (mouseCounter < 20)
            {
                this.IsMouseVisible = true;
                mouseCounter++;
            }
            else
            {
                this.IsMouseVisible = false;
            }


            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            int originalX = (int)this.mapViewportCamera.Location.X;
            int originalY = (int)this.mapViewportCamera.Location.Y;


            HandleMapScrolling(originalY, originalX, newKeyboardState);
            oldKeyboardState = newKeyboardState;

            SwitchToNewGameStateViewIfNeeded();
            gameCursor.Update(gameTime);
            base.Update(gameTime);
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }


        private void HandleMapScrolling(int originalY, int originalX, KeyboardState newKeyboardState)
        {
            int scrollAmount = 10;
            int mouseScrollThreshold = 30;

            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            float zoomChangeAmount = 0.2f;
            if (mouseState.Position.X > defaultViewport.Width - mouseScrollThreshold)
            {
                int newX = (int) (this.mapViewportCamera.Location.X + 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.X < mouseScrollThreshold)
            {
                int newX = (int) (this.mapViewportCamera.Location.X - 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.Y > defaultViewport.Height - mouseScrollThreshold)
            {
                int newY = (int) (this.mapViewportCamera.Location.Y + 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (mouseState.Position.Y < mouseScrollThreshold)
            {
                int newY = (int) (this.mapViewportCamera.Location.Y - 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }

            else if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
            {
                int newX = (int) (this.mapViewportCamera.Location.X + scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
            {
                int newX = (int) (this.mapViewportCamera.Location.X - scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
            {
                int newY = (int) (this.mapViewportCamera.Location.Y + scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
            {
                int newY = (int) (this.mapViewportCamera.Location.Y - scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemPlus) && newKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                float newZoom = this.mapViewportCamera.Zoom + zoomChangeAmount;
                this.mapViewportCamera.Zoom = newZoom;
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemMinus) && newKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                float newZoom = this.mapViewportCamera.Zoom - zoomChangeAmount;
                this.mapViewportCamera.Zoom = newZoom;
            }

            SnapMapCameraToBounds();
        }

        private void SwitchToNewGameStateViewIfNeeded()
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
            GraphicsDevice.Clear(Color.Crimson);

            DrawMap(gameTime);
            DrawToolbar(gameTime);
            DrawGameCursor(gameTime);

            GraphicsDevice.Viewport = defaultViewport;
            base.Draw(gameTime);
        }

        private void DrawMap(GameTime gameTime)
        {
            GraphicsDevice.Viewport = mapViewport;
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);

            this.currentGameStateView.Draw(gameTime, spriteBatch);
            gdiBarracksView.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void DrawToolbar(GameTime gameTime)
        {
            GraphicsDevice.Viewport = toolbarViewport;
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                toolbarViewportCamera.TransformMatrix);
            minigunnerIconView.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void DrawGameCursor(GameTime gameTime)
        {
            GraphicsDevice.Viewport = defaultViewport;
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect);
            gameCursor.Draw(gameTime, spriteBatch);
            spriteBatch.End();


        }






        internal Minigunner AddGdiMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {

            Point positionInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);
            return AddGdiMinigunner(positionInWorldCoordinates);
        }


        internal Minigunner AddGdiMinigunner(Point positionInWorldCoordinates)
        {

            Minigunner newMinigunner =  GameWorld.instance.AddGdiMinigunner(positionInWorldCoordinates);

            // TODO:  In future, decouple always adding a view when adding a minigunner
            // to enable running headless with no UI
            MinigunnerView newMinigunnerView = new GdiMinigunnerView(newMinigunner);
            GdiMinigunnerViewList.Add(newMinigunnerView);
            return newMinigunner;
        }

        internal Sandbag AddSandbag(int xInMapSquareCoordinates, int yInMapSquareCoordinates, int sandbagType)
        {

            int xInWorldCoordinates = xInMapSquareCoordinates * GameWorld.MAP_TILE_WIDTH + 12;
            int yInWorldCoordinates = yInMapSquareCoordinates * GameWorld.MAP_TILE_HEIGHT + 12;

            Sandbag newSandbag = new Sandbag(xInWorldCoordinates, yInWorldCoordinates, sandbagType);
            GameWorld.instance.sandbagList.Add(newSandbag);

            SandbagView newSandbagView = new SandbagView(newSandbag);
            sandbagViewList.Add(newSandbagView);
            return newSandbag;
        }

        internal Minigunner AddNodMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates, bool aiIsOn)
        {

            Point positionInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            return AddNodMinigunner(positionInWorldCoordinates, aiIsOn);
        }


        internal GDIBarracksView AddGDIBarracksViewAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            int xInWorldCoordinates = positionInMapSquareCoordinates.X * GameWorld.MAP_TILE_WIDTH;
            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * GameWorld.MAP_TILE_HEIGHT;

            Point positionInWorldCoordinates = new Point(xInWorldCoordinates, yInWorldCoordinates);

            GDIBarracksView gdiBarracksView = new GDIBarracksView(positionInWorldCoordinates);
            return gdiBarracksView;

        }



        internal Minigunner AddNodMinigunner(Point positionInWorldCoordinates, bool aiIsOn)
        {
            Minigunner newMinigunner = gameWorld.AddNodMinigunner(positionInWorldCoordinates, aiIsOn);

            MinigunnerView newMinigunnerView = new NodMinigunnerView(newMinigunner);
            NodMinigunnerViewList.Add(newMinigunnerView);
            return newMinigunner;
        }

        public GameState HandleReset()
        {
            gdiMinigunnerViewList.Clear();
            nodMinigunnerViewList.Clear();
            sandbagViewList.Clear();
            // TODO:  Bogus stuff here
            // Have to reset world first, before then resetting navigation navigationGraph
            // because navigation navigationGraph depends on what's in the game world
            // and sandbags were not getting cleared before navigation navigationGraph was updated
            GameState newGameState = gameWorld.HandleReset();
            gameWorld.InitializeNavigationGraph();
            return newGameState;
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


        public Vector2 ConvertWorldCoordinatesToScreenCoordinates(Vector2 positionInWorldCoordinates)
        {
            return Vector2.Transform(positionInWorldCoordinates, MikeAndConquerGame.instance.mapViewportCamera.TransformMatrix);
        }

        public Vector2 ConvertScreenLocationToWorldLocation(Vector2 screenLocation)
        {
            return Vector2.Transform(screenLocation, Matrix.Invert(MikeAndConquerGame.instance.mapViewportCamera.TransformMatrix));
        }


        //        // TODO:  Where does this method go?
        //        public Point ConvertMapSquareCoordinatesToWorldCoordinates(Point positionInMapSquareCoordinates)
        //        {
        //
        //            int xInWorldCoordinates = positionInMapSquareCoordinates.X * 24 + 12;
        //            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * 24 + 12;
        //
        //            return new Point(xInWorldCoordinates, yInWorldCoordinates);
        //        }



    }




}
