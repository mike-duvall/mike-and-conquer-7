
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
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using MinigunnerView = mike_and_conquer.gameview.MinigunnerView;
using GdiMinigunnerView = mike_and_conquer.gameview.GdiMinigunnerView;
using NodMinigunnerView = mike_and_conquer.gameview.NodMinigunnerView;
using SandbagView = mike_and_conquer.gameview.SandbagView;

using BasicMapSquare = mike_and_conquer.gameview.BasicMapSquare;


using Camera2D = mike_and_conquer_6.Camera2D;

using Point = Microsoft.Xna.Framework.Point;
using Vector2 = Microsoft.Xna.Framework.Vector2;

using Serilog;




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

        private List<BasicMapSquareView> basicMapSquareViewList;

        private List<SandbagView> sandbagViewList;

        private GameStateView currentGameStateView;

        public GameCursor gameCursor;

        public UnitSelectionBox unitSelectionBox;

        private GameState currentGameState;

        public List<BasicMapSquareView> BasicMapSquareViewList
        {
            get { return basicMapSquareViewList; }
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

        public TextureListMap TextureListMap
        {
            get { return textureListMap; }
        }

        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
        }

        private RAISpriteFrameManager raiSpriteFrameManager;
        private SpriteSheet spriteSheet;


        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private TextureListMap textureListMap;

        private bool testMode;

        private int borderSize = 0;

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

            basicMapSquareViewList = new List<BasicMapSquareView>();

            gdiMinigunnerViewList = new List<MinigunnerView>();
            nodMinigunnerViewList = new List<MinigunnerView>();

            sandbagViewList = new List<SandbagView>();

            textureListMap = new TextureListMap();

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


        public static Vector2 ConvertWorldCoordinatesToScreenCoordinates(Vector2 positionInWorldCoordinates)
        {
            return Vector2.Transform(positionInWorldCoordinates, MikeAndConquerGame.instance.mapViewportCamera.TransformMatrix);
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
//        protected override void Initialize()
//        {
//            // TODO: Add your initialization logic here
//            base.Initialize();
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

            foreach(BasicMapSquare basicMapSquare in this.gameWorld.BasicMapSquareList)
            {
                BasicMapSquareView basicMapSquareView = new BasicMapSquareView(basicMapSquare);
                this.basicMapSquareViewList.Add(basicMapSquareView);
            }

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            gameWorld.Initialize();
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

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //            textureListMap.LoadSpriteListFromShpFile(GdiMinigunnerView.SPRITE_KEY, GdiMinigunnerView.SHP_FILE_NAME, GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);
            //            textureListMap.LoadSpriteListFromShpFile(NodMinigunnerView.SPRITE_KEY, GdiMinigunnerView.SHP_FILE_NAME, NodMinigunnerView.SHP_FILE_COLOR_MAPPER);
            //            textureListMap.LoadSpriteListFromShpFile(SandbagView.SPRITE_KEY, SandbagView.SHP_FILE_NAME, SandbagView.SHP_FILE_COLOR_MAPPER);



            textureListMap.LoadSpriteListFromShpFile(MinigunnerIconView.SPRITE_KEY, MinigunnerIconView.SHP_FILE_NAME,
                MinigunnerIconView.SHP_FILE_COLOR_MAPPER);

            textureListMap.LoadSpriteListFromShpFile(GDIBarracksView.SPRITE_KEY, GDIBarracksView.SHP_FILE_NAME, GDIBarracksView.SHP_FILE_COLOR_MAPPER);

            LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY, "Accomplished");
            LoadSingleTextureFromFile(gameobjects.MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");
            LoadSingleTextureFromFile(gameobjects.DestinationSquare.SPRITE_KEY, gameobjects.DestinationSquare.SPRITE_KEY);


            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GdiMinigunnerView.SHP_FILE_NAME,GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);

            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GdiMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GdiMinigunnerView.SHP_FILE_NAME),
                GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);

            spriteSheet.LoadUnitFramesFromSpriteFrames(
                NodMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(NodMinigunnerView.SHP_FILE_NAME),
                NodMinigunnerView.SHP_FILE_COLOR_MAPPER);


            raiSpriteFrameManager.LoadAllTexturesFromShpFile(SandbagView.SHP_FILE_NAME, SandbagView.SHP_FILE_COLOR_MAPPER);

            spriteSheet.LoadUnitFramesFromSpriteFrames(
                SandbagView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(SandbagView.SHP_FILE_NAME),
                SandbagView.SHP_FILE_COLOR_MAPPER);



            raiSpriteFrameManager.LoadAllTexturesFromTmpFile(TextureListMap.CLEAR1_SHP);

            spriteSheet.LoadMapTileFramesFromSpriteFrames(
                TextureListMap.CLEAR1_SHP,
                raiSpriteFrameManager.GetSpriteFramesForMapTile(TextureListMap.CLEAR1_SHP));


//             Pickup here:
            // Use code above to load all unit textures and all map textures
            // Then update existing code to use SpriteSheet instead of of TextureListMap
            // May need UnitSprite vs MapTileSprite to handle differences
            // Have already done GdiMinigunnerView

            // Consider LoadMapTexturesNew(), LoadUnitTexturesNew(), LoadOtherTextures() methods

        }

        private void LoadMapTextures()
        {
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.CLEAR1_SHP);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D04_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D09_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D13_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D15_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D20_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D21_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D23_TEM);

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.P07_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.P08_TEM);

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S09_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S10_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S11_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S12_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S14_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S22_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S29_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S32_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S34_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S35_TEM);

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH1_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH2_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH3_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH4_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH5_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH6_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH9_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH10_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH17_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.SH18_TEM);

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.W1_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.W2_TEM);

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
            int widthOfMapSquare = 24;
            int widthOfMapInWorldSpace = gameWorld.gameMap.numColumns * widthOfMapSquare;

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
            int heightOfMapSquare = 24;
            int heightOfMapInWorldSpace = gameWorld.gameMap.numRows * heightOfMapSquare;
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

        private int mouseCounter = 0;
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


        public Point ConvertMapSquareCoordinatesToWorldCoordinates(Point positionInMapSquareCoordinates)
        {

            int xInWorldCoordinates = positionInMapSquareCoordinates.X * 24 + 12;
            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * 24 + 12;

            return new Point(xInWorldCoordinates,yInWorldCoordinates);

        }


        internal Minigunner AddGdiMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            Point positionInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

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

            int xInWorldCoordinates = xInMapSquareCoordinates * 24 + 12;
            int yInWorldCoordinates = yInMapSquareCoordinates * 24 + 12;

            Sandbag newSandbag = new Sandbag(xInWorldCoordinates, yInWorldCoordinates, sandbagType);
            GameWorld.instance.sandbagList.Add(newSandbag);

            SandbagView newSandbagView = new SandbagView(newSandbag);
            sandbagViewList.Add(newSandbagView);
            return newSandbag;
        }

        internal Minigunner AddNodMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates, bool aiIsOn)
        {
            Point positionInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            return AddNodMinigunner(positionInWorldCoordinates, aiIsOn);
        }


        internal GDIBarracksView AddGDIBarracksViewAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            int xInWorldCoordinates = positionInMapSquareCoordinates.X * 24;
            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * 24;

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


        private void LoadSingleTextureFromFile(string key, string fileName)
        {
            SpriteTextureList spriteTextureList = new SpriteTextureList();
            Texture2D texture2D = Content.Load<Texture2D>(fileName);


            ShpFileImage shpFileImage = new ShpFileImage(texture2D,null,null);
            spriteTextureList.shpFileImageList.Add(shpFileImage);
            spriteTextureList.textureWidth = texture2D.Width;
            spriteTextureList.textureHeight = texture2D.Height;

            TextureListMap.AddTextureList(key, spriteTextureList);
        }


        public GameState HandleReset()
        {
            gdiMinigunnerViewList.Clear();
            nodMinigunnerViewList.Clear();
            sandbagViewList.Clear();
            // TODO:  Bogus stuff here
            // Have to reset world first, before then resetting navigation graph
            // because navigation graph depends on what's in the game world
            // and sandbags were not getting cleared before navigation graph was updated
            GameState newGameState = gameWorld.HandleReset();
            gameWorld.InitializeNavigationGraph();
            return newGameState;
        }

        public BasicMapSquareView FindMapSquareView(int xWorldCoordinate, int yWorldCoordinate)
        {

            foreach (BasicMapSquareView nextBasicMapSquareView in this.basicMapSquareViewList)
            {
                BasicMapSquare basicMapSquare = nextBasicMapSquareView.myBasicMapSquare;
                if (basicMapSquare.ContainsPoint(new Point(xWorldCoordinate, yWorldCoordinate)))
                {
                    return nextBasicMapSquareView;
                }
            }
            throw new Exception("Unable to find BasicMapSquare at coordinates, x:" + xWorldCoordinate + ", y:" + yWorldCoordinate);

        }


    }

}
